using System;
using System.Collections.Generic;
using System.Linq;
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
            var devices = new List<T>();

            using (var searcher = new ManagementObjectSearcher(Query))
            {
                foreach (var obj in searcher.Get())
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




//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Management;

//namespace AffinityProgram.Queries.Abstract
//{
//    public abstract class DeviceInfo
//    {
//        public string Query { get; }

//        protected DeviceInfo(string query)
//        {
//            Query = query;
//        }

//        public List<T> GetDevices<T>()
//        {
//            var devices = new HashSet<T>();
//            var printedAntecedents = new HashSet<string>();

//            using (var searcher = new ManagementObjectSearcher(Query))
//            {
//                using (var collection = searcher.Get())
//                {
//                    foreach (var obj in collection)
//                    {
//                        switch (Query)
//                        {
//                            case string q when q.Contains("Win32_USBControllerDevice"):
//                                var antecedent = (string)obj.GetPropertyValue("Antecedent");

//                                if (printedAntecedents.Add(antecedent))
//                                {
//                                    devices.Add((T)Activator.CreateInstance(typeof(T), antecedent));
//                                }

//                                devices.Add((T)Activator.CreateInstance(typeof(T), (string)obj.GetPropertyValue("Dependent")));
//                                break;
//                            default:
//                                devices.Add((T)Activator.CreateInstance(typeof(T), (string)obj.GetPropertyValue("PnPDeviceID")));
//                                break;
//                        }
//                    }
//                }
//            }

//            return devices.ToList();
//        }
//    }
//}













////ÇALIŞAN
////using System;
////using System.Collections.Generic;
////using System.Linq;
////using System.Management;
////using System.Text;
////using System.Threading.Tasks;

////namespace AffinityProgram.Queries.Abstracrt
////{
////    public abstract class DeviceInfo
////    {
////        protected string Query;

////        protected DeviceInfo(string query)
////        {
////            Query = query;
////        }


////        public List<T> GetDevices<T>()
////        {
////            var devices = new List<T>();
////            var printedAntecedents = new HashSet<string>();

////            using (var searcher = new ManagementObjectSearcher(Query))
////            {
////                using (var collection = searcher.Get())
////                {
////                    foreach (var device in collection)
////                    {
////                        var antecedent = (string)device.GetPropertyValue("Antecedent");

////                        if (Query.Contains("Win32_USBControllerDevice"))
////                        {
////                            if (printedAntecedents.Add(antecedent))
////                            {
////                                devices.Add((T)Activator.CreateInstance(typeof(T), antecedent));
////                            }

////                            devices.Add((T)Activator.CreateInstance(typeof(T), (string)device.GetPropertyValue("Dependent")));
////                        }
////                        else
////                        {
////                            devices.Add((T)Activator.CreateInstance(typeof(T), (string)device.GetPropertyValue("PnPDeviceID")));
////                        }
////                    }
////                }
////            }

////            return devices;
////        }
////    }
////}
