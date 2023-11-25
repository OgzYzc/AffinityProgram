﻿using System;
using System.IO;
using System.Threading;

namespace AddDSCP
{
    public class PathManager
    {
        static string JSONPath = Path.Combine(Path.GetTempPath(), "appinfo.json");
        public static void Run()
        {
            try
            {
                if (File.Exists(JSONPath))
                {
                    File.Delete(JSONPath);
                    Run();
                }
                else
                {
                    CreateJSON();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        private static void CreateJSON()
        {
            Console.WriteLine("Creating a new JSON file");
            Thread.Sleep(1500);
            Console.Clear();
            //if json is still exists delete it
            if (File.Exists(JSONPath))
            {
                File.Delete(JSONPath);
            }
            else
            {
                using (VDFConverter vdfReader = new VDFConverter(Path.Combine(GetSteamPath(), "appcache", "appinfo.vdf")))
                {
                    vdfReader.Transform();
                }
            }
            ExitMethod();
        }
        private static void ExitMethod()
        {
            Console.Clear();

            Console.WriteLine("DSCP values added succesfully.");
            // Adding this just for fun
            Random rnd = new Random();
            double delaySeconds = rnd.NextDouble() * 3;
            Console.WriteLine($"Returning to the main menu in {delaySeconds} seconds.");

            int milliseconds = (int)(delaySeconds * 1000);
            int fractionalMilliseconds = (int)((delaySeconds - Math.Floor(delaySeconds)) * 1000);

            Thread.Sleep(milliseconds + fractionalMilliseconds);

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
        public static string GetSteamPath()
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(Path.Combine("SOFTWARE", "Valve", "Steam"));

                if (key != null)
                {
                    var steamPath = key.GetValue("SteamPath");

                    if (steamPath != null)
                    {
                        key.Close();
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
