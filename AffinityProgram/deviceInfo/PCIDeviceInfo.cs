using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AffinityProgram.deviceInfo
{
    internal class PCIDeviceInfo
    {
        public PCIDeviceInfo(string deviceID)
        {
            this.DeviceID = deviceID;
        }
        public string DeviceID { get; private set; }
    }
}
