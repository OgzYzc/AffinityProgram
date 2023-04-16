using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AffinityProgram.Model
{
    internal class Model_NicDevices
    {
        public Model_NicDevices(string NetworkDeviceID)
        {
            this.DeviceID = NetworkDeviceID ?? throw new ArgumentNullException(nameof(NetworkDeviceID));
        }
        public string DeviceID { get; }
    }
}
