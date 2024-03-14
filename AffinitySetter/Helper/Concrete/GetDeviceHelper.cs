using AffinitySetter.Model.Devices;

namespace AffinitySetter.Helper.Concrete;

public class GetDeviceHelper
{
    public List<string> GetDeviceID<T>(List<T> deviceInstanceIdList, string value) where T : BaseModel
    {
        return deviceInstanceIdList
                .Select(item => value.Replace("$i", item.DeviceID))
                .ToList();
    }
}
