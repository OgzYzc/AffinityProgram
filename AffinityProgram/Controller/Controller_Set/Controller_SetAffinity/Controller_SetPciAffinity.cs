using AffinityProgram.Controller.Controller_Set.Controller_SetAffinity;
using AffinityProgram.Find_Core;
using AffinityProgram.Model;
using AffinityProgram.Queries.Concrete;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Security.Principal;

namespace AffinityProgram.Controller.Controller_Set
{
    internal class Controller_SetPciAffinity
    {
        public Controller_SetPciAffinity()
        {
            bool continueProgram = CPPCError();
            if (continueProgram)
            {
                try
                {

                    var registryPath = new Model_RegistryPath(@"SYSTEM\CurrentControlSet\Enum\$i\Device Parameters\Interrupt Management\Affinity Policy");

                    var devices = GetPciDevices();

                    var regSecurity = CreateRegistrySecurity();

                    foreach (var device in devices)
                    {
                        if (string.IsNullOrEmpty(device.DeviceID))
                            continue;

                        var keyPath = registryPath.RegistryPath.Replace("$i", device.DeviceID);

                        using (var key = Registry.LocalMachine.CreateSubKey(keyPath, RegistryKeyPermissionCheck.ReadWriteSubTree, regSecurity))
                        {
                            if (Find_Core_CPPC.GPUhexBytes == null)
                            {

                                Console.Clear();
                                Console.WriteLine("Would you like to add PCI bridge affinity as well? (May be more performant). Press Enter to add.");

                                ConsoleKeyInfo consoleKeyInfo = Console.ReadKey(true);

                                if (consoleKeyInfo.Key == ConsoleKey.Enter)
                                {
                                    Console.Clear();
                                    AddPciBridgeAffinity(regSecurity, registryPath);
                                    AddAffintyNoCPPC(key);
                                }
                                else
                                {
                                    Console.Clear();
                                    AddAffintyNoCPPC(key);
                                }
                            }
                            else
                            {
                                key.SetValue("AssignmentSetOverride", Find_Core_CPPC.GPUhexBytes, RegistryValueKind.Binary);
                                key.SetValue("DevicePolicy", "4", RegistryValueKind.DWord);
                                Console.WriteLine("Affinity added.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
                return;

        }

        private List<Model_PciDevices> GetPciDevices()
        {
            var deviceInfo = new Query_PciDevices();
            return deviceInfo.GetDevices<Model_PciDevices>();
        }


        private RegistrySecurity CreateRegistrySecurity()
        {
            var regSecurity = new RegistrySecurity();
            regSecurity.AddAccessRule(new RegistryAccessRule(new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null), RegistryRights.FullControl, InheritanceFlags.None, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
            return regSecurity;
        }
        private static void AddAffintyNoCPPC(RegistryKey key)
        {
            byte[] assignmentSetOverrideValue = View.MainMenu.isSmtEnabled ? new byte[] { 64 } : new byte[] { 16 };

            key.SetValue("AssignmentSetOverride", assignmentSetOverrideValue, RegistryValueKind.Binary);
            key.SetValue("DevicePolicy", "4", RegistryValueKind.DWord);
            Console.WriteLine("Pci affinity added.");
        }

        private void AddPciBridgeAffinity(RegistrySecurity regSecurity, Model_RegistryPath registryPath)
        {
            Controller_ListPciBridges controller_SetPciBridgeAffinity = new Controller_ListPciBridges();
            controller_SetPciBridgeAffinity.SetPciBridgeAffinity();

            byte[] assignmentSetOverrideValue = View.MainMenu.isSmtEnabled ? new byte[] { 64 } : new byte[] { 16 };

            foreach (var item in Controller_ListPciBridges.PciBridgeList)
            {
                var keyPath = registryPath.RegistryPath.Replace("$i", item.ToString());

                using (var key = Registry.LocalMachine.CreateSubKey(keyPath, RegistryKeyPermissionCheck.ReadWriteSubTree, regSecurity))
                {
                    key.SetValue("AssignmentSetOverride", assignmentSetOverrideValue, RegistryValueKind.Binary);
                    key.SetValue("DevicePolicy", "4", RegistryValueKind.DWord);
                    Console.WriteLine("Pci bridge affinity added.");
                }
            }
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
