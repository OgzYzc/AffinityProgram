﻿using AffinityProgram.Model;
using AffinityProgram.Queries.Concrete;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;

namespace AffinityProgram.Controller.Controller_SetMsiLimit
{
    internal class Controller_SetUsbMsiLimit
    {
        public Controller_SetUsbMsiLimit()
        {
            try
            {
                Model_RegistryPath registryPath = new Model_RegistryPath(@"SYSTEM\CurrentControlSet\Enum\$i\Device Parameters\Interrupt Management\MessageSignaledInterruptProperties");

                List<Model_UsbDevices> devices = GetUsbDevices();

                RegistrySecurity regSecurity = CreateRegistrySecurity();

                foreach (Model_UsbDevices device in devices)
                {
                    if (!string.IsNullOrEmpty(device.DeviceID))
                    {
                        SetMessageLimitForDevice(device, registryPath, regSecurity);
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
        private void SetMessageLimitForDevice(Model_UsbDevices device, Model_RegistryPath registryPath, RegistrySecurity regSecurity)
        {
            List<string> controllerDeviceIDs = new List<string>
            {
                @"PCI\VEN_1022&DEV_149C",
                @"PCI\VEN_1022&DEV_43EE"
            };

            if (controllerDeviceIDs.Any(id => device.DeviceID.Contains(id)))
            {
                string keyPath = registryPath.RegistryPath.Replace("$i", device.DeviceID);
                using (RegistryKey key = Registry.LocalMachine.CreateSubKey(keyPath, RegistryKeyPermissionCheck.ReadWriteSubTree, regSecurity))
                {
                    key.SetValue("MessageNumberLimit", "8", RegistryValueKind.DWord);
                    Console.WriteLine("Message limit added.");
                }
            }
        }
    }
}
