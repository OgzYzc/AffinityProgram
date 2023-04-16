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
    internal class Controller_SetNicDevices
    {
        public Controller_SetNicDevices()
        {
            Concrete_RegistryPath concreteRegistryPath = new Concrete_RegistryPath();
            string RegistryPath = concreteRegistryPath.registryPath;

            var deviceInfo = new Query_NicDevices();
            var devices = deviceInfo.GetDevices<Model_NicDevices>();
            foreach (var device in devices)
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(RegistryPath.Replace("$i", device.DeviceID), true))
                {
                    if (key != null)
                    {
                        key.SetValue("DevicePolicy", "5", RegistryValueKind.DWord);
                        Console.WriteLine("Affinity added.");
                    }
                }
            }
        }
    }
}
