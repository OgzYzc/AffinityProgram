using System.Runtime.InteropServices;
using AffinitySetter.Queries.Abstract;


namespace AffinitySetter.Queries.Concrete;

internal static class DeviceInfo
{
    [DllImport("setupapi.dll", SetLastError = true)]
    internal static extern nint SetupDiGetClassDevs(ref Guid classGuid, string enumerator, nint hwndParent, uint flags);

    [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Ansi)]
    internal static extern bool SetupDiEnumDeviceInfo(nint deviceInfoSet, uint memberIndex, ref SP_DEVINFO_DATA deviceInfoData);

    [DllImport("setupapi.dll", CharSet = CharSet.Ansi, SetLastError = true)]
    internal static extern bool SetupDiGetDeviceInstanceId(nint DeviceInfoSet, ref SP_DEVINFO_DATA DeviceInfoData, nint DeviceInstanceId, uint DeviceInstanceIdSize, out uint RequiredSize);

    [DllImport("setupapi.dll", SetLastError = true)]
    internal static extern bool SetupDiDestroyDeviceInfoList(nint DeviceInfoSet);

    internal const int DIGCF_PRESENT = 0x00000000; // 0 -> Get only present devices

    internal struct SP_DEVINFO_DATA
    {
        internal uint cbSize;
        internal Guid ClassGuid;
        internal uint DevInst;
        internal nint Reserved;
    }
}
internal class DeviceInfo<T> : IDeviceInfoService<T>
{
    public List<T> GetDevices(Guid classGuid, Func<string, T> converter)
    {
        var deviceInstanceIds = new List<T>();
        nint deviceInfoSet = DeviceInfo.SetupDiGetClassDevs(ref classGuid, null, nint.Zero, DeviceInfo.DIGCF_PRESENT);
        if (deviceInfoSet != nint.Zero && deviceInfoSet.ToInt64() != -1)
        {
            try
            {
                DeviceInfo.SP_DEVINFO_DATA deviceInfoData = new DeviceInfo.SP_DEVINFO_DATA();
                deviceInfoData.cbSize = (uint)Marshal.SizeOf(deviceInfoData);

                for (uint i = 0; DeviceInfo.SetupDiEnumDeviceInfo(deviceInfoSet, i, ref deviceInfoData); i++)
                {
                    uint requiredSize = 0;
                    DeviceInfo.SetupDiGetDeviceInstanceId(deviceInfoSet, ref deviceInfoData, nint.Zero, 0, out requiredSize);
                    if (requiredSize > 0)
                    {
                        nint buffer = Marshal.AllocHGlobal((int)requiredSize);
                        if (DeviceInfo.SetupDiGetDeviceInstanceId(deviceInfoSet, ref deviceInfoData, buffer, requiredSize, out _))
                        {
                            string deviceId = Marshal.PtrToStringAnsi(buffer);
                            T deviceModel = converter(deviceId);
                            deviceInstanceIds.Add(deviceModel);
                        }
                        Marshal.FreeHGlobal(buffer);
                    }
                }
            }
            finally
            {
                DeviceInfo.SetupDiDestroyDeviceInfoList(deviceInfoSet);
            }
        }
        return deviceInstanceIds;
    }
}