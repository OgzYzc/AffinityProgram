﻿using AffinityProgram.Model;
using AffinityProgram.Queries.Concrete;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace AffinityProgram.Controller.Controller_SetInterruptPriority
{
    internal class Controller_SetUsbInterruptPriority
    {
        public Controller_SetUsbInterruptPriority()
        {
            try
            {
                var registryPath = new Model_RegistryPath(@"SYSTEM\CurrentControlSet\Enum\$i\Device Parameters\Interrupt Management\Affinity Policy");

                var deviceInfo = new Query_UsbDevices();
                var devices = deviceInfo.GetDevices<Model_UsbDevices>();

                var regSecurity = new RegistrySecurity();
                regSecurity.AddAccessRule(new RegistryAccessRule(new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null), RegistryRights.FullControl, InheritanceFlags.None, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));

                foreach (var device in devices)
                {
                    if (!string.IsNullOrEmpty(device.DeviceID))
                    {
                        if (device.DeviceID.Contains(@"PCI\VEN_1022&DEV_149C"))
                        {
                            var keyPath = registryPath.RegistryPath.Replace("$i", device.DeviceID);
                            using (var key = Registry.LocalMachine.CreateSubKey(keyPath, RegistryKeyPermissionCheck.ReadWriteSubTree, regSecurity))
                            {
                                //Priority Low
                                key.SetValue("DevicePriority", "1", RegistryValueKind.DWord);
                                Console.WriteLine("Priority added.");
                            }
                        }
                        else if (device.DeviceID.Contains(@"PCI\VEN_1022&DEV_43EE"))
                        {
                            var keyPath = registryPath.RegistryPath.Replace("$i", device.DeviceID);
                            using (var key = Registry.LocalMachine.CreateSubKey(keyPath, RegistryKeyPermissionCheck.ReadWriteSubTree, regSecurity))
                            {
                                //Priority Normal
                                key.SetValue("DevicePriority", "2", RegistryValueKind.DWord);
                                Console.WriteLine("Priority added.");
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