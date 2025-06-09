using System.Diagnostics;

namespace Base.Utility.Concrete;
public class CommandLineUtility : ICommandLineUtilityService
{
    public string StartCMD(string command, bool captureOutput = false)
    {
        using (Process process = new Process())
        {
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = $"/c \"{command}\"";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardOutput = captureOutput;
            process.StartInfo.CreateNoWindow = true;

            process.Start();

            string output = captureOutput ? process.StandardOutput.ReadToEnd() : string.Empty;
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                Console.WriteLine("Command failed with error:\n" + error);
            }

            return output;
        }
    }
}
