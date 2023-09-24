using System;
using System.Collections.Generic;
using System.Management;

namespace AffinityProgram.Queries.Abstract
{
    public abstract class DeviceInfo
    {
        private static readonly HashSet<string> printedAntecedents = new HashSet<string>();

        public string Query { get; }

        protected DeviceInfo(string query)
        {
            Query = query;
        }

        public List<T> GetDevices<T>()
        {
            List<T> devices = new List<T>();

            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(Query))
            {
                foreach (ManagementBaseObject obj in searcher.Get())
                {
                    switch (Query)
                    {
                        case string q when q.Contains("Win32_USBControllerDevice"):
                            if (obj.GetPropertyValue("Antecedent") is string antecedent && printedAntecedents.Add(antecedent))
                            {
                                devices.Add((T)Activator.CreateInstance(typeof(T), antecedent));
                            }

                            devices.Add((T)Activator.CreateInstance(typeof(T), obj.GetPropertyValue("Dependent") as string));
                            break;

                        default:
                            devices.Add((T)Activator.CreateInstance(typeof(T), obj.GetPropertyValue("PnPDeviceID") as string));
                            break;
                    }
                }
            }

            return devices;
        }
    }
}
