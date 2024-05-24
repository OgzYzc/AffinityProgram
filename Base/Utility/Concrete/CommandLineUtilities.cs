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
            process.StartInfo.Arguments = $"/c \"{command}\"";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;
            process.Start();

            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if (process.ExitCode == 0)
            {
                Console.WriteLine("Command executed successfully:\n");
            }
            else
            {
                Console.WriteLine("Command failed with error:\n" + error);
            }
        }

        GC.Collect();
        GC.WaitForPendingFinalizers();
    }
}
