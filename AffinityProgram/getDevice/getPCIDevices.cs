using AffinityProgram.deviceInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace AffinityProgram.getDevice
{
    internal class getPCIDevices
    {
        public static List<PCIDeviceInfo> GetPCIDevices()
        {
            List<PCIDeviceInfo> devices = new List<PCIDeviceInfo>();

            using (var searcher = new ManagementObjectSearcher(
                @"Select * From Win32_VideoController"))
            {
                using (var collection = searcher.Get())
                {
                    foreach (var device in collection)
                    {
                        devices.Add(new PCIDeviceInfo((string)device.GetPropertyValue("PnPDeviceID")));
                    }
                }
            }
            return devices;
        }
    }
}
