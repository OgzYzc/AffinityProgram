using AffinityProgram.Model;
using AffinityProgram.Queries.Concrete;
using System;

namespace AffinityProgram.Controller.Controller_List
{
    internal class Controller_ListPciDevices
    {
        public Controller_ListPciDevices()
        {
            var deviceInfo = new Query_PciDevices();
            var devices = deviceInfo.GetDevices<Model_PciDevices>();
            foreach (var device in devices)
            {
                Console.WriteLine(device.DeviceID);
            }
        }
    }
}
