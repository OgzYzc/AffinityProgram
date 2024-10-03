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
        AddMockQoS();
        RemoveMockQoS();
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

    public void AddMockQoS()
    {
        string createCommand = "powershell -Command \"New-NetQosPolicy -Name 'MockQoS' -AppPathNameMatchCondition '*' -IPProtocolMatchCondition Both -IPSrcPortStartMatchCondition 50000 -IPSrcPortEndMatchCondition 50019 -DSCPAction 46 -NetworkProfile All\"";
        _commandLineUtilityService.StartCMD(createCommand, false);
    }

    public void RemoveMockQoS()
    {
        string createCommand = "powershell -Command \"Remove-NetQosPolicy -Name 'Teams Audio' -Confirm:$false -ErrorAction SilentlyContinue\"";
        _commandLineUtilityService.StartCMD(createCommand, false);
    }


}
