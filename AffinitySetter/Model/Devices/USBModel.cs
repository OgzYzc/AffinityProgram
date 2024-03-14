namespace AffinitySetter.Model.Devices;

internal class USBModel : BaseModel
{
    private string _deviceID;

    public new string DeviceID
    {
        get => _deviceID;
        set
        {
            if (!value.StartsWith("SWD"))
                _deviceID = value;
        }
    }
    public USBModel(string UsbDeviceID) : base(UsbDeviceID) => DeviceID = UsbDeviceID;

}
