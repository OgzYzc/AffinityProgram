using AffinityProgram.Controller.Concrete;
using AffinityProgram.Model;
using AffinityProgram.Queries.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AffinityProgram.Controller.Controller_List
{
    internal class Controller_ListPciDevices
    {
        public Controller_ListPciDevices()
        {
            Concrete_RegistryPath _RegistryPath = new Concrete_RegistryPath();
            string _Name = _RegistryPath.registryPath;

            var deviceInfo = new Query_PciDevices();
            var devices = deviceInfo.GetDevices<Model_PciDevices>();
            foreach (var device in devices)
            {
                Console.WriteLine(device.DeviceID);
            }
        }
    }
}
