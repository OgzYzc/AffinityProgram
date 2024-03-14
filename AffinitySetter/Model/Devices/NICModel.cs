namespace AffinitySetter.Model.Devices;

internal class NICModel : BaseModel
{
    private string _deviceID;

    public new string DeviceID
    {
        get => _deviceID;
        set
        {
            if (value.StartsWith("PCI"))
                _deviceID = value;
        }
    }

    public NICModel(string NicDeviceID) : base(NicDeviceID) => DeviceID = NicDeviceID;
}
