using AffinityProgram.Controller.Concrete;
using AffinityProgram.Model;
using AffinityProgram.Queries.Concrete;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AffinityProgram.Controller.Controller_Set
{
    internal class Controller_SetUsbDevices
    {
        public Controller_SetUsbDevices()
        {
            Concrete_RegistryPath concreteRegistryPath = new Concrete_RegistryPath();
            string RegistryPath = concreteRegistryPath.registryPath;

            var deviceInfo = new Query_UsbDevices();
            var devices = deviceInfo.GetDevices<Model_UsbDevices>();
            foreach (var device in devices)
            {
                if (!String.IsNullOrEmpty(device.DeviceID))
                {
                    using (RegistryKey key = Registry.LocalMachine.OpenSubKey(RegistryPath.Replace("$i", device.DeviceID), true))
                    {
                        if (key != null)
                        {
                            key.SetValue("AssignmentSetOverride", new Byte[] { 32 }, RegistryValueKind.Binary);
                            key.SetValue("DevicePolicy", "4", RegistryValueKind.DWord);
                            Console.WriteLine("Affinity added.");

                        }
                    }
                }
            }
        }
    }
}
