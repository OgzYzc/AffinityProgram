using AffinityProgram.Model;
using AffinityProgram.Queries.Concrete;
using System;

namespace AffinityProgram.Controller.Controller_List
{
    internal class Controller_ListNicDevices
    {
        public Controller_ListNicDevices()
        {
            var deviceInfo = new Query_NicDevices();
            var devices = deviceInfo.GetDevices<Model_NicDevices>();
            foreach (var device in devices)
            {
                Console.WriteLine(device.DeviceID);
            }
        }
    }
}
