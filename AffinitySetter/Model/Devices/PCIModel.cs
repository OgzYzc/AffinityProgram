namespace AffinitySetter.Model.Devices;

public class PCIModel : BaseModel
{
    public PCIModel(string PciDeviceID) : base(PciDeviceID)
    {
        DeviceID = PciDeviceID;
    }
    public new string DeviceID { get; }
}
