using AffinityProgram.data;
using AffinityProgram.deviceInfo;
using AffinityProgram.getDevice;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AffinityProgram.setDeviceAffinity
{
    internal class setNICDevices
    {
        public static void setNICDevicesAffinity()
        {
            var networkDevice = getNICDevices.GetNetworkDevice();
            var registryPath = registryData.registryPath;
            foreach (NetworkAdapterInfo networkdevice in networkDevice)
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registryPath.Replace("$i", networkdevice.DeviceID), true))
                {
                    if (key != null)
                    {
                        key.SetValue("DevicePolicy", "5", RegistryValueKind.DWord);
                    }
                }
                Console.WriteLine("Affinity added.");
            }
        }
    }
}
