namespace Base.Utility;
public interface ICommandLineUtilityService
{
    string StartCMD(string command, bool captureOutput);
}
