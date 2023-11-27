using AffinityProgram.Find_Core;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;

namespace AffinityProgram.Controller.Controller_SetNicRegistry
{
    internal class Controller_SetNicRegistry
    {
        private const string ndisRegistryPath = @"SYSTEM\CurrentControlSet\Services\NDIS\Parameters";
        private const string tcpipRegistryPath = @"SYSTEM\CurrentControlSet\Services\Tcpip";
        private const string adapterRegistryPath = @"SYSTEM\CurrentControlSet\Control\Class\{4d36e972-e325-11ce-bfc1-08002be10318}\";
        private readonly string driverPath = adapterRegistryPath + GetDriverPath();

        public Controller_SetNicRegistry()
        {
            bool continueProgram = CPPCError();
            if (continueProgram)
            {
                try
                {
                    Console.Clear();
                    setNdisKeyValues();
                    setTcpipKeyValues();
                    setDriverKeyValues();

                    Thread.Sleep(3000);
                    Console.Clear();
                    Controller_SetNicPowershell.Controller_SetNicPowershell.Run();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
                return;

        }

        private RegistrySecurity getRegistrySecurity()
        {
            RegistrySecurity regSecurity = new RegistrySecurity();
            regSecurity.AddAccessRule(new RegistryAccessRule(new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null),
                RegistryRights.FullControl, InheritanceFlags.None, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
            return regSecurity;
        }

        private void setNdisKeyValues()
        {
            using (RegistryKey ndisKey = Registry.LocalMachine.CreateSubKey(ndisRegistryPath, RegistryKeyPermissionCheck.ReadWriteSubTree, getRegistrySecurity()))
            {
                if (Find_Core_CPPC.selectedCoreNIC == null)
                {
                    ndisKey.SetValue("RssBaseCpu", View.MainMenu.isSmtEnabled ? "4" : "2", RegistryValueKind.DWord);

                }
                else
                {
                    double selectedCore = Math.Log(Find_Core_CPPC.selectedCoreNIC[0], 2);
                    ndisKey.SetValue("RssBaseCpu", selectedCore, RegistryValueKind.DWord);
                }

                // Use max 2 core on either choice
                ndisKey.SetValue("MaxNumRssCpus", "2", RegistryValueKind.DWord);
            }

            Console.WriteLine("Registry key added successfully.\nExecuting powershell.");
        }

        private void setTcpipKeyValues()
        {

            using (RegistryKey tcpipKey = Registry.LocalMachine.CreateSubKey(tcpipRegistryPath, RegistryKeyPermissionCheck.ReadWriteSubTree, getRegistrySecurity()))
            {
                tcpipKey.SetValue("DisableTaskOffload", "0", RegistryValueKind.DWord);
                tcpipKey.SetValue("SackOpts", "0", RegistryValueKind.DWord);
            }
        }

        private void setDriverKeyValues()
        {
            Dictionary<string, string> smtRssValues = new Dictionary<string, string>()
            {
                { "*RSS", "1" },
                //Using profile "Conservative" for less interrupt to CPU
                { "*RSSProfile", "5" },
                { "*RssBaseProcNumber", "4" },
                { "*MaxRssProcessors", "2" },
                { "*NumaNodeId", "0" },
                { "*NumRssQueues", "2" },
                { "*RssBaseProcGroup", "0" },
                { "*RssMaxProcNumber", "6" },
                { "*RssMaxProcGroup", "0" },
            };

            Dictionary<string, string> nonSmtRssValues = new Dictionary<string, string>()
            {
                { "*RSS", "1" },
                { "*RSSProfile", "5" },
                { "*RssBaseProcNumber", "2" },
                { "*MaxRssProcessors", "2" },
                { "*NumaNodeId", "0" },
                { "*NumRssQueues", "2" },
                { "*RssBaseProcGroup", "0" },
                { "*RssMaxProcNumber", "3" },
                { "*RssMaxProcGroup", "0" },
            };

            Dictionary<string, string> commonNicValues = new Dictionary<string, string>()
            {
                //Latency
                { "*EEE", "0" },
                { "*FlowControl", "0" },
                { "*InterruptModeration", "0" },
                { "*LsoV1IPv4", "0" },
                { "*LsoV2IPv4", "0" },
                { "*LsoV1IPv6", "0" },
                { "*LsoV2IPv6", "0" },
                { "*PMNSOffload", "0" },
                { "*PMARPOffload", "0" },
                { "*ReceiveBuffers", "1600" },
                { "*TransmitBuffers", "1600" },
                { "DMACoalescing", "0" },
                { "EnableLLI", "2" }, //TCP PSH flag
                { "EnableDCA", "1" },
                { "EnableModernStandby", "0" },
                { "EnablePME", "0" },
                { "ITR", "0" },
                { "TxIntDelay", "0" },
                //These ones are listed in intel linux driver documentation. Not sure it works.
                //downloadmirror.intel.com/15817/eng/readme.txt
                { "RxIntDelay", "0" },
                { "RxAbsIntDelay", "0" },
                { "TxAbsIntDelay", "0" },

            };

            using (RegistryKey driverKey = Registry.LocalMachine.CreateSubKey(driverPath, RegistryKeyPermissionCheck.ReadWriteSubTree, getRegistrySecurity()))
            {
                foreach (KeyValuePair<string, string> entry in commonNicValues)
                {
                    driverKey.SetValue(entry.Key, entry.Value, RegistryValueKind.String);

                    //Add Rss values depending on SMT
                    Dictionary<string, string> selectedRssValues = View.MainMenu.isSmtEnabled ? smtRssValues : nonSmtRssValues;

                    foreach (KeyValuePair<string, string> value in selectedRssValues)
                        driverKey.SetValue(value.Key, value.Value, RegistryValueKind.String);

                }
            }

            Console.WriteLine("Driver values are added.");
        }

        private static string GetDriverPath()
        {
            List<string> ProviderList = new List<string> {
            "Intel",
            "Realtek",
            "Marvell",
            "Mellanox"
            };

            foreach (var Provider in ProviderList)
            {
                try
                {
                    using (RegistryKey classKey = Registry.LocalMachine.OpenSubKey(adapterRegistryPath))
                    {
                        foreach (var subKeyName in classKey?.GetSubKeyNames())
                        {
                            if (subKeyName != "Configuration" && subKeyName != "Properties")
                            {
                                using (RegistryKey productKey = classKey.OpenSubKey(subKeyName))
                                {
                                    string keyValue = productKey?.GetValue("ProviderName")?.ToString();
                                    if (keyValue != null && keyValue.Equals(Provider.ToString(), StringComparison.OrdinalIgnoreCase))
                                    {
                                        return subKeyName;
                                    }
                                }

                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error : {ex.Message}");
                }
            }
            return null;
        }

        private bool CPPCError()
        {
            if (Find_Core_CPPC.selectedCoreNIC == null)
            {
                Console.WriteLine("You are adding affinity without using CPPC. " +
                                    "If you enabled CPPC, go back to the menu and press 'Find best core' then come back." +
                                    " Or you can add predetermined affinity. Press Enter for adding Predetermined affinity.");

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    return true;
                }
                else
                    return false;
            }
            else
                return true;
        }
    }
}
