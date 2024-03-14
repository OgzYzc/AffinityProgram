namespace FindCore.Model;
public class USBByteArrayModel
{
    public USBByteArrayModel(byte[] byteArray)
    {
        USBByteArray = byteArray;
    }

    public static byte[] USBByteArray { get; set; }
}
