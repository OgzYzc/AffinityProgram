using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AffinityProgram.Model
{
    internal class Model_NicDevices
    {
        public Model_NicDevices(string NetworkDeviceID = null)
        {
            this.DeviceID = NetworkDeviceID ?? throw new ArgumentNullException(nameof(NetworkDeviceID));
        }

        public Model_NicDevices(int[] NicArray = null)
        {
            this.coreNICArray = NicArray;
        }

        public int[] coreNICArray { get; set; }
        public string DeviceID { get; }
    }
}
