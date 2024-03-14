namespace Base.Constants;

public class GuidClasses
{
    //https://learn.microsoft.com/en-us/windows-hardware/drivers/install/system-defined-device-setup-classes-available-to-vendors
    //https://learn.microsoft.com/en-us/windows-hardware/drivers/install/system-defined-device-setup-classes-reserved-for-system-use

    public static readonly Guid NicGuidClass = new Guid("{4d36e972-e325-11ce-bfc1-08002be10318}");
    public static readonly Guid PciGuidClass = new Guid("{4d36e968-e325-11ce-bfc1-08002be10318}");
    public static readonly Guid UsbGuidClass = new Guid("{36fc9e60-c465-11cf-8056-444553540000}");
    public static readonly Guid HidGuidClass = new Guid("{745a17a0-74d3-11d0-b6fe-00a0c90f57da}");

}
