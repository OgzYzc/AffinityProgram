using AffinityProgram.Model;
using AffinityProgram.Queries.Concrete;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Security.Principal;

namespace AffinityProgram.Controller.Controller_Del
{
    internal class Controller_RemUsbAffinity
    {
        public Controller_RemUsbAffinity()
        {
            try
            {
                Model_RegistryPath registryPath = new Model_RegistryPath(@"SYSTEM\CurrentControlSet\Enum\$i\Device Parameters\Interrupt Management\Affinity Policy");

                List<Model_UsbDevices> devices = GetUsbDevices();

                RegistrySecurity regSecurity = CreateRegistrySecurity();

                foreach (Model_UsbDevices device in devices)
                {
                    if (!string.IsNullOrEmpty(device.DeviceID))
                    {
                        string keyPath = registryPath.RegistryPath.Replace("$i", device.DeviceID);
                        using (RegistryKey key = Registry.LocalMachine.CreateSubKey(keyPath, RegistryKeyPermissionCheck.ReadWriteSubTree, regSecurity))
                        {
                            key.DeleteValue("AssignmentSetOverride");
                            key.DeleteValue("DevicePolicy");
                            Console.WriteLine("Affinity removed.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private List<Model_UsbDevices> GetUsbDevices()
        {
            Query_UsbDevices deviceInfo = new Query_UsbDevices();
            return deviceInfo.GetDevices<Model_UsbDevices>();
        }


        private RegistrySecurity CreateRegistrySecurity()
        {
            RegistrySecurity regSecurity = new RegistrySecurity();
            regSecurity.AddAccessRule(new RegistryAccessRule(new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null), RegistryRights.FullControl, InheritanceFlags.None, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
            return regSecurity;
        }
    }
}
