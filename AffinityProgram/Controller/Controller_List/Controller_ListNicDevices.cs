using AffinityProgram.Model;
using AffinityProgram.Queries.Concrete;
using System;
using System.Collections.Generic;

namespace AffinityProgram.Controller.Controller_List
{
    internal class Controller_ListNicDevices
    {
        public Controller_ListNicDevices()
        {
            Query_NicDevices deviceInfo = new Query_NicDevices();
            List<Model_NicDevices> devices = deviceInfo.GetDevices<Model_NicDevices>();
            foreach (Model_NicDevices device in devices)
            {
                Console.WriteLine(device.DeviceID);
            }
        }
    }
}
