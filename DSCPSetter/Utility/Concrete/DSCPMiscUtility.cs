using Base.Constants;
using Base.Utility;
using DSCPSetter.Helper.Abstract;
using DSCPSetter.Utility.Abstract;
using System.Diagnostics;

namespace DSCPSetter.Utility.Concrete;
public class DSCPMiscUtility : IDSCPMiscUtilityService
{
    ICommandLineUtilityService _commandLineUtilityService;
    public DSCPMiscUtility(ICommandLineUtilityService commandLineUtilityService)
    {
        _commandLineUtilityService = commandLineUtilityService;
    }

    public void AddSchedule()
    {
        // ಠ╭╮ಠ

        string executablePath = @"C:\Windows\System32\gpupdate.exe";
        string arguments = "/force /wait:0";
        string taskName = "GPUpdateTask";

        string command = $"/C schtasks /Create /TN \"{taskName}\" /TR \"{executablePath} {arguments}\" /SC ONLOGON /ru SYSTEM";

        _commandLineUtilityService.StartCMD(command);
    }
}
