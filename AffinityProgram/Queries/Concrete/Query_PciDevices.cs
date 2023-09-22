using AffinityProgram.Queries.Abstract;

namespace AffinityProgram.Queries.Concrete
{
    internal class Query_PciDevices : DeviceInfo
    {
        public Query_PciDevices() : base($"Select * From Win32_VideoController")
        {
        }
    }
}
