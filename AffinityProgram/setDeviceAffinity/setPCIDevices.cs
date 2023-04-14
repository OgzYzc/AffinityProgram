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
    internal class setPCIDevices
    {
        public static void setPCIDevicesAffinity()
        {
            var pciDevices = getPCIDevices.GetPCIDevices();
            var registryPath = registryData.registryPath;
            foreach (PCIDeviceInfo pcidevice in pciDevices)
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registryPath.Replace("$i", pcidevice.DeviceID), true))
                {
                    if (key != null)
                    {
                        key.SetValue("AssignmentSetOverride", new Byte[] { 02 }, RegistryValueKind.Binary);
                        key.SetValue("DevicePolicy", "4", RegistryValueKind.DWord);
                    }
                }
                Console.WriteLine("Affinity added.");
            }
        }
    }
}
