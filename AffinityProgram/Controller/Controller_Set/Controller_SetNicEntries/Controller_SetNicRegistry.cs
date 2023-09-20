using AffinityProgram.Controller.Controller_SetNicPowershell;
using AffinityProgram.Find_Core;
using AffinityProgram.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Management.Automation;
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
            try
            {
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
                    Console.WriteLine("You are adding affinity without using CPPC. " +
                        "If you enabled CPPC go back to menu and press 'Find best core' then come back." +
                        "Or you can add predetermined affinity. Press Enter for adding Predetermined affinity.");

                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                    if (keyInfo.Key == ConsoleKey.Enter)
                    {
                        if (View.MainMenu.isSmtEnabled)
                        {
                            ndisKey.SetValue("RssBaseCpu", "4", RegistryValueKind.DWord);
                        }
                        else
                        {
                            ndisKey.SetValue("RssBaseCpu", "0", RegistryValueKind.DWord);
                        }
                    }
                    else
                        return;
                }
                else
                {
                    var selectedCore = Math.Log(Find_Core_CPPC.selectedCoreNIC[0], 2);
                    ndisKey.SetValue("RssBaseCpu", selectedCore, RegistryValueKind.DWord);
                }

                // Use max 2 core on either choice
                ndisKey.SetValue("MaxNumRssCpus", "4", RegistryValueKind.DWord);
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
            var nicValues = new Dictionary<string, string>()
        {
                //RSS                
                { "*RSS", "1" },
                { "*RSSProfile", "4" },
                { "*RssBaseProcNumber", "0" },
                { "*MaxRssProcessors", "4" },
                { "*NumaNodeId", "0" },
                { "*NumRssQueues", "4" },
                { "*RssBaseProcGroup", "0" },
                { "*RssMaxProcNumber", "3" },
                { "*RssMaxProcGroup", "0" },

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
                foreach (var entry in nicValues)
                {
                    driverKey.SetValue(entry.Key, entry.Value, RegistryValueKind.String);

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
    }
}
