using Base.Constants;
using Base.Utility;
using DSCPSetter.Helper.Abstract;
using DSCPSetter.Utility.Abstract;
using System;
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
        string executablePath = @"C:\Windows\System32\gpupdate.exe";
        string arguments = "/force /wait:0";
        string taskName = "GPUpdateTask";

        string queryCommand = $"schtasks /Query /TN \"{taskName}\"";

        string output = _commandLineUtilityService.StartCMD(queryCommand, captureOutput: true);

        if (output.Contains(taskName))
        {
            Console.WriteLine($"Task '{taskName}' already exists. No action taken.");
            return;
        }

        string createCommand = $"schtasks /Create /TN \"{taskName}\" /TR \"{executablePath} {arguments}\" /SC ONLOGON /RU SYSTEM";
        _commandLineUtilityService.StartCMD(createCommand, true);

        Console.WriteLine(Messages.TaskAdded);
    }

}
