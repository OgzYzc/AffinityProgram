using AffinityProgram.Queries.Abstract;

namespace AffinityProgram.Queries.Concrete
{
    internal class Query_NicDevices : DeviceInfo
    {
        public Query_NicDevices() : base($"Select * From Win32_NetworkAdapter Where PnPDeviceID Like '%PCI%'")
        {
        }
    }
}
