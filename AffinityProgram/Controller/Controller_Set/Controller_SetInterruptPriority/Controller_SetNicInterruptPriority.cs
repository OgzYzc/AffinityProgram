using AffinityProgram.Model;
using AffinityProgram.Queries.Concrete;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Security.Principal;

namespace AffinityProgram.Controller.Controller_SetInterruptPriority
{
    internal class Controller_SetNicInterruptPriority
    {
        public Controller_SetNicInterruptPriority()
        {
            try
            {

                Model_RegistryPath registryPath = new Model_RegistryPath(@"SYSTEM\CurrentControlSet\Enum\$i\Device Parameters\Interrupt Management\Affinity Policy");

                List<Model_NicDevices> devices = GetNicDevices();

                RegistrySecurity regSecurity = CreateRegistrySecurity();

                foreach (Model_NicDevices device in devices)
                {
                    if (!string.IsNullOrEmpty(device.DeviceID))
                    {
                        string keyPath = registryPath.RegistryPath.Replace("$i", device.DeviceID);
                        using (RegistryKey key = Registry.LocalMachine.CreateSubKey(keyPath, RegistryKeyPermissionCheck.ReadWriteSubTree, regSecurity))
                        {
                            //Priority Normal
                            key.SetValue("DevicePriority", "2", RegistryValueKind.DWord);
                            Console.WriteLine("Priority added.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private List<Model_NicDevices> GetNicDevices()
        {
            Query_NicDevices deviceInfo = new Query_NicDevices();
            return deviceInfo.GetDevices<Model_NicDevices>();
        }

        private RegistrySecurity CreateRegistrySecurity()
        {
            RegistrySecurity regSecurity = new RegistrySecurity();
            regSecurity.AddAccessRule(new RegistryAccessRule(new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null), RegistryRights.FullControl, InheritanceFlags.None, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
            return regSecurity;
        }
    }
}
