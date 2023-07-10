using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AffinityProgram.Model
{
    internal class Model_PciDevices
    {
        public Model_PciDevices(string PciDeviceID = null)
        {
            this.DeviceID = PciDeviceID;            
        }
        public Model_PciDevices(byte[] GPUArray = null)
        {
            this.coreGPUArray = GPUArray;
        }
        public string DeviceID { get; }

        public byte[] coreGPUArray { get; set; }
    }
}
