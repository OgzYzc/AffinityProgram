using AffinityProgram.Model;
using AffinityProgram.Queries.Concrete;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Security.Principal;

namespace AffinityProgram.Controller.Controller_SetMsiLimit
{
    internal class Controller_SetPciMsiLimit
    {
        public Controller_SetPciMsiLimit()
        {
            try
            {
                var registryPath = new Model_RegistryPath(@"SYSTEM\CurrentControlSet\Enum\$i\Device Parameters\Interrupt Management\MessageSignaledInterruptProperties");

                var devices = GetPciDevices();

                var regSecurity = CreateRegistrySecurity();

                foreach (var device in devices)
                {
                    if (!string.IsNullOrEmpty(device.DeviceID))
                    {
                        var keyPath = registryPath.RegistryPath.Replace("$i", device.DeviceID);
                        using (var key = Registry.LocalMachine.CreateSubKey(keyPath, RegistryKeyPermissionCheck.ReadWriteSubTree, regSecurity))
                        {
                            key.SetValue("MessageNumberLimit", "1", RegistryValueKind.DWord);
                            Console.WriteLine("Message limit added.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
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
    }
}
