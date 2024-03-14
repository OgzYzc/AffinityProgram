namespace FindCore.Model;
public class NICByteArrayModel
{
    public NICByteArrayModel(byte[] byteArray)
    {
        NICByteArray = byteArray;
    }

    public static byte[] NICByteArray { get; set; }
}
