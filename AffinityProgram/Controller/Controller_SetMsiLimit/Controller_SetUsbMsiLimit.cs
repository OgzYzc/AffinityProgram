using AffinityProgram.Controller.Concrete;
using AffinityProgram.Model;
using AffinityProgram.Queries.Concrete;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace AffinityProgram.Controller.Controller_SetMsiLimit
{
    internal class Controller_SetUsbMsiLimit
    {
        public Controller_SetUsbMsiLimit()
        {
            try
            {
                var concreteRegistryPath = new Concrete_RegistryPath();
                string registryPath = concreteRegistryPath.MsiLimitRegistryPath;

                var deviceInfo = new Query_UsbDevices();
                var devices = deviceInfo.GetDevices<Model_UsbDevices>();

                var regSecurity = new RegistrySecurity();
                regSecurity.AddAccessRule(new RegistryAccessRule(new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null), RegistryRights.FullControl, InheritanceFlags.None, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));

                foreach (var device in devices)
                {
                    if (!string.IsNullOrEmpty(device.DeviceID))
                    {
                        //Using if-else because don't want to add msi limit to every usb port but controllers.
                        if (device.DeviceID.Contains(@"PCI\VEN_1022&DEV_149C"))
                        {
                            var keyPath = registryPath.Replace("$i", device.DeviceID);
                            using (var key = Registry.LocalMachine.CreateSubKey(keyPath, RegistryKeyPermissionCheck.ReadWriteSubTree, regSecurity))
                            {                                
                                key.SetValue("MessageNumberLimit", "8", RegistryValueKind.DWord);
                                Console.WriteLine("Message limit added.");
                            }
                        }
                        else if (device.DeviceID.Contains(@"PCI\VEN_1022&DEV_43EE"))
                        {
                            var keyPath = registryPath.Replace("$i", device.DeviceID);
                            using (var key = Registry.LocalMachine.CreateSubKey(keyPath, RegistryKeyPermissionCheck.ReadWriteSubTree, regSecurity))
                            {                                
                                key.SetValue("MessageNumberLimit", "8", RegistryValueKind.DWord);
                                Console.WriteLine("Message limit added.");
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
