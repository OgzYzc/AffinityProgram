using AffinityProgram.Model;
using AffinityProgram.Queries.Concrete;
using System;

namespace AffinityProgram.Controller.Controller_List
{
    internal class Controller_ListUsbDevices
    {
        public Controller_ListUsbDevices()
        {
            Query_UsbDevices deviceInfo = new Query_UsbDevices();
            System.Collections.Generic.List<Model_UsbDevices> devices = deviceInfo.GetDevices<Model_UsbDevices>();
            foreach (Model_UsbDevices device in devices)
            {
                if (!String.IsNullOrEmpty(device.DeviceID))
                {
                    Console.WriteLine(device.DeviceID);
                }
            }
        }
    }
}
