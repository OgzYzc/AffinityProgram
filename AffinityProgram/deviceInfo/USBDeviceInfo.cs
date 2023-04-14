using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AffinityProgram.deviceInfo
{
    internal class USBDeviceInfo
    {
        public USBDeviceInfo(string deviceID)
        {
            if (!deviceID.Contains("SWD"))
            {
                this.DeviceID = deviceID.Substring(54).Trim(new char[] { (char)34 }).Replace(@"\\", @"\");
            }
        }
        public string DeviceID { get; private set; }
    }
}
