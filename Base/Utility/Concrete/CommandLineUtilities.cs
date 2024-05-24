using Base.Constants;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Utility.Concrete;
public class CommandLineUtility : ICommandLineUtilityService
{
    public void StartCMD(string command)
    {
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
