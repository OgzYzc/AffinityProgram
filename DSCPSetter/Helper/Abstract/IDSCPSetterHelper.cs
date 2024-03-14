namespace DSCPSetter.Helper.Abstract;
public interface IDSCPSetterHelper
{
    void RunDSCP();
    void ReadJsonFiles();
    void RunVDFConverter();
    void AddDSCPRegistrySettings();
    void AddScheduledTask();
}
