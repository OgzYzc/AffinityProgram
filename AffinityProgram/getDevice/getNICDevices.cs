using AffinityProgram.deviceInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace AffinityProgram.getDevice
{
    internal class getNICDevices
    {
        public static List<NetworkAdapterInfo> GetNetworkDevice()
        {
            List<NetworkAdapterInfo> devices = new List<NetworkAdapterInfo>();

            using (var searcher = new ManagementObjectSearcher(
                @"Select * From Win32_NetworkAdapter Where PnPDeviceID Like '%PCI%'"))
            {
                using (var collection = searcher.Get())
                {
                    foreach (var device in collection)
                    {
                        devices.Add(new NetworkAdapterInfo((string)device.GetPropertyValue("PnPDeviceID")));
                    }
                }
            }
            return devices;
        }
    }
}
