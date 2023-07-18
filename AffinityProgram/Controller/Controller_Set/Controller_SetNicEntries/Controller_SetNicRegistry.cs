using AffinityProgram.Controller.Controller_SetNicPowershell;
using AffinityProgram.Find_Core;
using AffinityProgram.Model;
using Microsoft.Win32;
using System;
using System.Management.Automation;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;

namespace AffinityProgram.Controller.Controller_SetNicRegistry
{
    internal class Controller_SetNicRegistry
    {
        public Controller_SetNicRegistry()
        {
            bool IsSmtEnabled = View.MainMenu.isSmtEnabled;

            try
            {
                //Adding RssBaseCpu to Ndis service
                var ndisRegistryPath = new Model_RegistryPath(@"SYSTEM\CurrentControlSet\Services\NDIS\Parameters");
                var tcpipRegistryPath = new Model_RegistryPath(@"SYSTEM\CurrentControlSet\Services\Tcpip");

                var regSecurity = new RegistrySecurity();
                regSecurity.AddAccessRule(new RegistryAccessRule(new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null), RegistryRights.FullControl, InheritanceFlags.None, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));

                using (var ndisKey = Registry.LocalMachine.CreateSubKey(ndisRegistryPath.RegistryPath, RegistryKeyPermissionCheck.ReadWriteSubTree, regSecurity))
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
                                ndisKey.SetValue("RssBaseCpu", "2", RegistryValueKind.DWord);
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

                    //Use max 2 core on either choice
                    ndisKey.SetValue("MaxNumRssCpus", "2", RegistryValueKind.DWord);

                    using (var tcpipKey = Registry.LocalMachine.CreateSubKey(tcpipRegistryPath.RegistryPath, RegistryKeyPermissionCheck.ReadWriteSubTree, regSecurity))
                    {
                        tcpipKey.SetValue("DisableTaskOffload","0",RegistryValueKind.DWord);
                    }

                    Console.WriteLine("Registry key added successfully.\nExecuting powershell.");
                }


                //Waiting 3 seconds for dramatic effect
                Thread.Sleep(3000);

                Controller_SetNicPowershell.Controller_SetNicPowershell.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
