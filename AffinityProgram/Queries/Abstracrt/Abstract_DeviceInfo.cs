using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace AffinityProgram.Queries.Abstracrt
{
    public abstract class DeviceInfo
    {
        protected string Query;

        protected DeviceInfo(string query)
        {
            Query = query;
        }

        public List<T> GetDevices<T>()
        {
            var devices = new List<T>();

            using (var searcher = new ManagementObjectSearcher(Query))
            {
                using (var collection = searcher.Get())
                {
                    foreach (var device in collection)
                    {
                        if (!Query.Contains("Win32_USBControllerDevice"))
                        {
                            devices.Add((T)Activator.CreateInstance(typeof(T), (string)device.GetPropertyValue("PnPDeviceID")));
                        }
                        else
                        {
                            devices.Add((T)Activator.CreateInstance(typeof(T), (string)device.GetPropertyValue("Dependent")));
                        }
                    }
                }
            }

            return devices;
        }
    }
}
