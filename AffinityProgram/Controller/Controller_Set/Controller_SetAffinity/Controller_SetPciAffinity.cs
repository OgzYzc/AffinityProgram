using AffinityProgram.Find_Core;
using AffinityProgram.Model;
using AffinityProgram.Queries.Concrete;
using Microsoft.Win32;
using System;
using System.Security.AccessControl;
using System.Security.Principal;

namespace AffinityProgram.Controller.Controller_Set
{
    internal class Controller_SetPciAffinity
    {
        public Controller_SetPciAffinity()
        {
            try
            {
                var registryPath = new Model_RegistryPath(@"SYSTEM\CurrentControlSet\Enum\$i\Device Parameters\Interrupt Management\Affinity Policy");

                var deviceInfo = new Query_PciDevices();
                var devices = deviceInfo.GetDevices<Model_PciDevices>();

                var regSecurity = new RegistrySecurity();
                regSecurity.AddAccessRule(new RegistryAccessRule(new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null), RegistryRights.FullControl, InheritanceFlags.None, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));

                foreach (var device in devices)
                {
                    if (!string.IsNullOrEmpty(device.DeviceID))
                    {
                        var keyPath = registryPath.RegistryPath.Replace("$i", device.DeviceID);
                        using (var key = Registry.LocalMachine.CreateSubKey(keyPath, RegistryKeyPermissionCheck.ReadWriteSubTree, regSecurity))
                        {
                            if (Find_Core_CPPC.GPUhexBytes == null)
                            {
                                Console.WriteLine("You are adding affinity without using CPPC. " +
                                    "If you enabled CPPC go back to menu and press 'Find best core' then come back." +
                                    "Or you can add predetermined affinity. Press Enter for adding Predetermined affinity.");

                                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                                if (keyInfo.Key == ConsoleKey.Enter)
                                {
                                    if (View.MainMenu.isSmtEnabled)
                                    {
                                        key.SetValue("AssignmentSetOverride", new byte[] { 04 }, RegistryValueKind.Binary);
                                        key.SetValue("DevicePolicy", "4", RegistryValueKind.DWord);
                                        Console.WriteLine("Affinity added.");
                                    }
                                    else
                                    {
                                        key.SetValue("AssignmentSetOverride", new byte[] { 02 }, RegistryValueKind.Binary);
                                        key.SetValue("DevicePolicy", "4", RegistryValueKind.DWord);
                                        Console.WriteLine("Affinity added.");
                                    }
                                }
                                else
                                    return;
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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
