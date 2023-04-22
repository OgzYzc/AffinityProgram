﻿using AffinityProgram.Controller.Concrete;
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

namespace AffinityProgram.Controller.Controller_SetInterruptPriority
{
    internal class Controller_SetPciInterruptPriority
    {
        public Controller_SetPciInterruptPriority()
        {
            try
            {
                var concreteRegistryPath = new Concrete_RegistryPath();
                var registryPath = concreteRegistryPath.registryPath;

                var deviceInfo = new Query_PciDevices();
                var devices = deviceInfo.GetDevices<Model_PciDevices>();

                var regSecurity = new RegistrySecurity();
                regSecurity.AddAccessRule(new RegistryAccessRule(new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null), RegistryRights.FullControl, InheritanceFlags.None, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));

                foreach (var device in devices)
                {
                    if (!string.IsNullOrEmpty(device.DeviceID))
                    {
                        var keyPath = registryPath.Replace("$i", device.DeviceID);
                        using (var key = Registry.LocalMachine.CreateSubKey(keyPath, RegistryKeyPermissionCheck.ReadWriteSubTree, regSecurity))
                        {
                            //Priority High
                            key.SetValue("DevicePriority", "3", RegistryValueKind.DWord);
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
    }
}
