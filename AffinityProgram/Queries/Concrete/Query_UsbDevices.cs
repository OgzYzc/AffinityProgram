using AffinityProgram.Queries.Abstract;

namespace AffinityProgram.Queries.Concrete
{
    internal class Query_UsbDevices : DeviceInfo
    {
        public Query_UsbDevices() : base($"Select * From Win32_USBControllerDevice")
        {
        }

    }
}
