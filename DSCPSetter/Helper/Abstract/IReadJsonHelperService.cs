namespace DSCPSetter.Helper.Abstract;
public interface IReadJsonHelperService
{
    void ReadAppInfo(string fileName);
    void ReadJSON(ReadOnlySpan<byte> json);
    void ReadLibraryFolder(string fileName);
}
