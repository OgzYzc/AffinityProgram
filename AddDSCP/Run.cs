using System;
using System.IO;
using System.Security.Policy;
using System.Threading;

namespace AddDSCP
{
    public class PathManager
    {
        public static void Run()
        {
            try
            {
                if (!File.Exists(Path.GetTempPath() + "appinfo.json"))
                {
                    CreateJSON();
                }
                else
                {
                    ReadJsonAppInfo.Read.ReadAppInfo(Path.GetTempPath() + "appinfo.json");
                    ReadJsonLibraryFolder.Read.ReadLibraryFolder(GetSteamPath() + @"\steamapps\libraryfolders.vdf");


                    AddDscpToRegistry.DscpReg.AddRegistry();


                    //Adding this just for fun
                    Random rnd = new Random();
                    double delaySeconds = rnd.NextDouble() * 3;
                    Console.WriteLine($"Returning to main menu in {delaySeconds} seconds.");

                    int milliseconds = (int)(delaySeconds * 1000);
                    int fractionalMilliseconds = (int)((delaySeconds - Math.Floor(delaySeconds)) * 1000);

                    Thread.Sleep(milliseconds + fractionalMilliseconds);

                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        static void CreateJSON()
        {
            Console.WriteLine("Creating a new JSON ile");
            Thread.Sleep(1500);

            using (VDFConverter vdfReader = new VDFConverter(GetSteamPath() + "/appcache/appinfo.vdf"))
            {
                vdfReader.Transform();
            }
        }

        public static string GetSteamPath()
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Valve\Steam");

                if (key != null)
                {
                    var steamPath = key.GetValue("SteamPath");

                    if (steamPath != null)
                    {
                        return steamPath.ToString();
                    }
                    else
                    {
                        throw new Exception("SteamPath value not found in the registry");
                    }
                }
                else
                {
                    throw new Exception("Steam registry key not found");
                }
            }
            else
            {
                throw new PlatformNotSupportedException();
            }
        }
    }
}
