using AffinityProgram.Queries.Abstracrt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AffinityProgram.Queries.Concrete
{
    internal class Query_NicDevices : DeviceInfo
    {
        public Query_NicDevices() : base($"Select * From Win32_NetworkAdapter Where PnPDeviceID Like '%PCI%'")
        {
        }
    }
}
