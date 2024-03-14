namespace AffinitySetter.Model.Devices;

public class BaseModel
{
    public BaseModel(string deviceId)
    {
        DeviceID = deviceId;
    }

    public string DeviceID { get; }
}
