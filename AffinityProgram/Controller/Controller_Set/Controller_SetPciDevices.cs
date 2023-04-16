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
    internal class Controller_SetPciDevices
    {
        public Controller_SetPciDevices()
        {
            Concrete_RegistryPath concreteRegistryPath = new Concrete_RegistryPath();
            string RegistryPath = concreteRegistryPath.registryPath;

            var deviceInfo = new Query_PciDevices();
            var devices = deviceInfo.GetDevices<Model_PciDevices>();
            foreach (var device in devices)
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(RegistryPath.Replace("$i", device.DeviceID), true))
                {
                    if (key != null)
                    {
                        key.SetValue("AssignmentSetOverride", new Byte[] { 02 }, RegistryValueKind.Binary);
                        key.SetValue("DevicePolicy", "4", RegistryValueKind.DWord);
                        Console.WriteLine("Affinity added.");
                    }
                }
            }
        }
    }
}
