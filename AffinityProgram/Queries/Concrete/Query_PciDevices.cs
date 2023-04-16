using AffinityProgram.Queries.Abstracrt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AffinityProgram.Queries.Concrete
{
    internal class Query_PciDevices : DeviceInfo
    {
        public Query_PciDevices() : base($"Select * From Win32_VideoController")
        {
        }
    }
}
