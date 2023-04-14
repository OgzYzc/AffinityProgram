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
    internal class setUSBDevices
    {
        public static void setUSBDevicesAffinity()
        {
            var usbDevices = getUSBDevices.GetUSBDevices();
            var registryPath = registryData.registryPath;
            foreach (USBDeviceInfo usbDevice in usbDevices)
            {
                if (!String.IsNullOrEmpty(usbDevice.DeviceID))
                {
                    using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registryPath.Replace("$i", usbDevice.DeviceID), true))
                    {
                        if (key != null)
                        {
                            key.SetValue("AssignmentSetOverride", new Byte[] { 32 }, RegistryValueKind.Binary);
                            key.SetValue("DevicePolicy", "4", RegistryValueKind.DWord);
                        }
                    }
                    Console.WriteLine("Affinity added.");
                }
            }
        }
    }
}
