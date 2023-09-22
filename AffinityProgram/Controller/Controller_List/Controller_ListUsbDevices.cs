using AffinityProgram.Model;
using AffinityProgram.Queries.Concrete;
using System;

namespace AffinityProgram.Controller.Controller_List
{
    internal class Controller_ListUsbDevices
    {
        public Controller_ListUsbDevices()
        {
            var deviceInfo = new Query_UsbDevices();
            var devices = deviceInfo.GetDevices<Model_UsbDevices>();
            foreach (var device in devices)
            {
                if (!String.IsNullOrEmpty(device.DeviceID))
                {
                    Console.WriteLine(device.DeviceID);
                }
            }
        }
    }
}
