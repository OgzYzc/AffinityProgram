using AffinityProgram.Model;
using AffinityProgram.Queries.Concrete;
using System;
using System.Collections.Generic;

namespace AffinityProgram.Controller.Controller_List
{
    internal class Controller_ListPciDevices
    {
        public Controller_ListPciDevices()
        {
            Query_PciDevices deviceInfo = new Query_PciDevices();
            List<Model_PciDevices> devices = deviceInfo.GetDevices<Model_PciDevices>();
            foreach (Model_PciDevices device in devices)
            {
                Console.WriteLine(device.DeviceID);
            }
        }
    }
}
