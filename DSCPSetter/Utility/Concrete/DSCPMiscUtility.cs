using Base.Constants;
using Base.Utility;
using DSCPSetter.Utility.Abstract;

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
        try
        {
            var result = _commandLineUtilityService.StartCMD(queryCommand,true);

            if (result.Contains("Folder:"))
            {
                Console.WriteLine($"Task '{taskName}' already exists. No action taken.");
                return;
            }
        }
        catch
        {
            return;
        }


        string createCommand = $"schtasks /Create /TN \"{taskName}\" /TR \"{executablePath} {arguments}\" /SC ONLOGON /RU SYSTEM";
        _commandLineUtilityService.StartCMD(createCommand, true);

        if (createCommand.Contains(taskName))
        {
            Console.WriteLine($"Task '{taskName}' already exists. No action taken.");
            return;
        }

        Console.WriteLine(Messages.TaskAdded);
    }

    public void AddMockQoS()
    {
        string createCommand = "powershell -Command \"New-NetQosPolicy -Name 'MockQoS' -AppPathNameMatchCondition '*' -IPProtocolMatchCondition Both -IPSrcPortStartMatchCondition 50000 -IPSrcPortEndMatchCondition 50019 -DSCPAction 46 -NetworkProfile All\"";
        _commandLineUtilityService.StartCMD(createCommand, false);
    }

    public void RemoveMockQoS()
    {
        string createCommand = "powershell -Command \"Remove-NetQosPolicy -Name 'MockQoS' -Confirm:$false -ErrorAction SilentlyContinue\"";
        _commandLineUtilityService.StartCMD(createCommand, false);
    }


}
