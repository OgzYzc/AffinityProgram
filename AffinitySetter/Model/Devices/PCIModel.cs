namespace AffinitySetter.Model.Devices;

internal class PCIModel : BaseModel
{
    public PCIModel(string PciDeviceID) : base(PciDeviceID)
    {
        DeviceID = PciDeviceID;
    }
    public new string DeviceID { get; }
}
