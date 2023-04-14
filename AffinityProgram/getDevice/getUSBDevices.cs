using AffinityProgram.deviceInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace AffinityProgram.getDevice
{
    internal class getUSBDevices
    {
        public static List<USBDeviceInfo> GetUSBDevices()
        {
            List<USBDeviceInfo> devices = new List<USBDeviceInfo>();

            using (var searcher = new ManagementObjectSearcher(
                @"Select * From Win32_USBControllerDevice"))
            {
                using (var collection = searcher.Get())
                {
                    foreach (var device in collection)
                    {
                        devices.Add(new USBDeviceInfo((string)device.GetPropertyValue("Dependent")));
                    }
                }
            }
            return devices;
        }
    }
}