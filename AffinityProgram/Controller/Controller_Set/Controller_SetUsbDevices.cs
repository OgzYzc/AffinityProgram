using AffinityProgram.Controller.Concrete;
using AffinityProgram.Model;
using AffinityProgram.Queries.Concrete;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace AffinityProgram.Controller.Controller_Set
{
    internal class Controller_SetUsbDevices
    {
        public Controller_SetUsbDevices()
        {
            try
            {
                var concreteRegistryPath = new Concrete_RegistryPath();
                var registryPath = concreteRegistryPath.registryPath;

                var deviceInfo = new Query_UsbDevices();
                var devices = deviceInfo.GetDevices<Model_UsbDevices>();

                var regSecurity = new RegistrySecurity();
                regSecurity.AddAccessRule(new RegistryAccessRule(new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null), RegistryRights.FullControl, InheritanceFlags.None, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));

                foreach (var device in devices)
                {
                    if (!string.IsNullOrEmpty(device.DeviceID))
                    {
                        var keyPath = registryPath.Replace("$i", device.DeviceID);
                        using (var key = Registry.LocalMachine.CreateSubKey(keyPath, RegistryKeyPermissionCheck.ReadWriteSubTree, regSecurity))
                        {
                            key.SetValue("AssignmentSetOverride", new Byte[] { 16 }, RegistryValueKind.Binary);
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
    }
}
