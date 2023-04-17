using AffinityProgram.Queries.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace AffinityProgram.Queries.Concrete
{
    internal class Query_UsbDevices : DeviceInfo
    {
        public Query_UsbDevices() : base($"Select * From Win32_USBControllerDevice")
        {
        }

    }
}
