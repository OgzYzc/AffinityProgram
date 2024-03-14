namespace FindCore.Model;
public class PCIByteArrayModel
{
    public PCIByteArrayModel(byte[] byteArray)
    {
        PCIByteArray = byteArray;

    }
    public static byte[]? PCIByteArray { get; set; }
}
