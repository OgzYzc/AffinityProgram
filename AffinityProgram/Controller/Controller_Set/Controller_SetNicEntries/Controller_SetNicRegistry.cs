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
        private const string adapterRegistryPath = @"SYSTEM\CurrentControlSet\Control\Class\{4d36e972-e325-11ce-bfc1-08002be10318}\0001"; //Need to work for this

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

                    // Waiting for a short time before executing the next action
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

        private RegistrySecurity GetRegistrySecurity()
        {
            var regSecurity = new RegistrySecurity();
            regSecurity.AddAccessRule(new RegistryAccessRule(new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null),
                RegistryRights.FullControl, InheritanceFlags.None, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
            return regSecurity;
        }

        private void setNdisKeyValues()
        {
            using (var ndisKey = Registry.LocalMachine.CreateSubKey(ndisRegistryPath, RegistryKeyPermissionCheck.ReadWriteSubTree, GetRegistrySecurity()))
            {
                if (Find_Core_CPPC.selectedCoreNIC == null)
                {
                    ndisKey.SetValue("RssBaseCpu", View.MainMenu.isSmtEnabled ? "2" : "0", RegistryValueKind.DWord);

                }
                else
                {
                    var selectedCore = Math.Log(Find_Core_CPPC.selectedCoreNIC[0], 2);
                    ndisKey.SetValue("RssBaseCpu", selectedCore, RegistryValueKind.DWord);
                }

                // Use max 2 core on either choice
                ndisKey.SetValue("MaxNumRssCpus", "2", RegistryValueKind.DWord);
            }

            Console.WriteLine("Registry key added successfully.\nExecuting powershell.");
        }

        private void setTcpipKeyValues()
        {
            using (var tcpipKey = Registry.LocalMachine.CreateSubKey(tcpipRegistryPath, RegistryKeyPermissionCheck.ReadWriteSubTree, GetRegistrySecurity()))
            {
                tcpipKey.SetValue("DisableTaskOffload", "0", RegistryValueKind.DWord);
            }
        }

        private void setDriverKeyValues()
        {
            var smtRssValues = new Dictionary<string, string>()
            {             
                { "*RSS", "1" },
                { "*RSSProfile", "4" },
                { "*RssBaseProcNumber", "0" },
                { "*MaxRssProcessors", "4" },
                { "*NumaNodeId", "0" },
                { "*NumRssQueues", "4" },
                { "*RssBaseProcGroup", "0" },
                { "*RssMaxProcNumber", "4" },
                { "*RssMaxProcGroup", "0" },
            };

            var nonSmtRssValues = new Dictionary<string, string>()
            {                
                { "*RSS", "1" },
                { "*RSSProfile", "4" },
                { "*RssBaseProcNumber", "0" },
                { "*MaxRssProcessors", "2" },
                { "*NumaNodeId", "0" },
                { "*NumRssQueues", "2" },
                { "*RssBaseProcGroup", "0" },
                { "*RssMaxProcNumber", "2" },
                { "*RssMaxProcGroup", "0" },
            };

            var commonNicValues = new Dictionary<string, string>()
            {
                //Latency
                { "*InterruptModeration", "0" },
                { "ITR", "0" },
                { "*LsoV1IPv4", "0" },
                { "*LsoV2IPv4", "0" },
                { "*LsoV1IPv6", "0" },
                { "*LsoV2IPv6", "0" },
                { "*FlowControl", "0" },
                { "TxIntDelay", "8" },
                { "EnableDCA", "1" },
                { "DMACoalescing", "0" },
                { "*TransmitBuffers", "1600" },
                { "*ReceiveBuffers", "1600" },
                { "EnableLLI", "1" },
                { "EnableModernStandby", "0" },
                { "EnablePME", "0" },
                { "*PMNSOffload", "0" },
                { "*PMARPOffload", "0" },                

                //These ones are listed in intel linux driver documentation. Not sure it works.
                //downloadmirror.intel.com/15817/eng/readme.txt
                { "RxIntDelay", "0" },
                { "RxAbsIntDelay", "0" },
                { "TxAbsIntDelay", "0" },

            };

            using (var driverKey = Registry.LocalMachine.CreateSubKey(adapterRegistryPath, RegistryKeyPermissionCheck.ReadWriteSubTree, GetRegistrySecurity()))
            {
                foreach (var entry in commonNicValues)
                {
                    driverKey.SetValue(entry.Key, entry.Value, RegistryValueKind.String);

                    //Add Rss values depending on SMT
                    var selectedRssValues = View.MainMenu.isSmtEnabled ? nonSmtRssValues : smtRssValues;
                    foreach (var value in selectedRssValues)
                        driverKey.SetValue(value.Key,value.Value, RegistryValueKind.String);

                    //Add everyport for low latency interrupt.
                    string[] lliPorts = new string[65535];
                    for (int i = 0; i < 65535; i++)
                    {
                        lliPorts[i] = (i + 1).ToString();
                    }

                    driverKey.SetValue("LLIPorts", lliPorts, RegistryValueKind.MultiString);
                }
            }

            // Set the value in the registry            

            Console.WriteLine("LLIPorts added to the registry.");

            Console.WriteLine("Driver values are added.");
        }
        private bool CPPCError()
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
    }
}
