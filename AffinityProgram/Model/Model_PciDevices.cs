using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AffinityProgram.Model
{
    internal class Model_PciDevices
    {
        public Model_PciDevices(string PciDeviceID)
        {
            this.DeviceID = PciDeviceID ?? throw new ArgumentNullException(nameof(PciDeviceID));
        }
        public string DeviceID { get; }
    }
}
