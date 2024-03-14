using Base.Constants;
using DSCPSetter.Utility.Abstract;
using System.Diagnostics;

namespace DSCPSetter.Utility.Concrete;
public class DSCPMiscUtility : IDSCPMiscUtilityService
{
    public void AddSchedule()
    {
        // ಠ╭╮ಠ

        string executablePath = @"C:\Windows\System32\gpupdate.exe";
        string arguments = "/force /wait:0";
        string taskName = "GPUpdateTask";

        string command = $"/C schtasks /Create /TN \"{taskName}\" /TR \"{executablePath} {arguments}\" /SC ONLOGON /ru SYSTEM";

        using (Process process = new Process())
        {
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = command;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();

            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            if (process.ExitCode == 0)
            {
                Console.WriteLine(Messages.TaskAdded);
            }
            else
            {
                Console.WriteLine("Failed to create scheduled task. Error: " + output);
            }
        }

        GC.Collect();
        GC.WaitForPendingFinalizers();
    }
}
