using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;

namespace AddDSCP
{
    internal class AddDscpToRegistry
    {
        public class DscpReg
        {
            private static Dictionary<string, int> gameNameCounter = new Dictionary<string, int>();

            public static void AddRegistry()
            {
                try
                {
                    //Clearing dictionary for not add additional key with unique number
                    gameNameCounter.Clear();
                    string path = @"SOFTWARE\Policies\Microsoft\Windows\QoS";

                    using (RegistryKey qosKey = Registry.LocalMachine.CreateSubKey(path, RegistryKeyPermissionCheck.ReadWriteSubTree, getRegistrySecurity()))
                    {
                        foreach (var item in ReadJsonLibraryFolder.Read.gamePaths)
                        {
                            string gameName = GetGameNameFromPath(item);

                            Console.WriteLine("Adding registry key for " + gameName);
                            using (RegistryKey gameKey = qosKey.CreateSubKey(gameName))
                            {
                                gameKey.SetValue("Application Name", item, RegistryValueKind.String);
                                gameKey.SetValue("DSCP Value", "46", RegistryValueKind.String);
                                gameKey.SetValue("Local IP", "*", RegistryValueKind.String);
                                gameKey.SetValue("Local IP Prefix Length", "*", RegistryValueKind.String);
                                gameKey.SetValue("Local Port", "*", RegistryValueKind.String);
                                gameKey.SetValue("Protocol", "*", RegistryValueKind.String);
                                gameKey.SetValue("Remote IP", "*", RegistryValueKind.String);
                                gameKey.SetValue("Remote IP Prefix Length", "*", RegistryValueKind.String);
                                gameKey.SetValue("Remote Port", "*", RegistryValueKind.String);
                                gameKey.SetValue("Throttle Rate", "-1", RegistryValueKind.String);
                                gameKey.SetValue("Version", "1.0", RegistryValueKind.String);
                            }
                        }
                        UpdateGroupPolicy();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error : {ex.Message}");
                }
            }
            static string GetGameNameFromPath(string path)
            {
                int commonIndex = path.IndexOf("common", StringComparison.OrdinalIgnoreCase);

                if (commonIndex >= 0)
                {
                    string substringAfterCommon = path.Substring(commonIndex + "common".Length + 1);

                    int nextBackslashIndex = substringAfterCommon.IndexOf(Path.DirectorySeparatorChar);

                    if (nextBackslashIndex >= 0)
                    {
                        string gameName = substringAfterCommon.Substring(0, nextBackslashIndex);

                        if (gameNameCounter.ContainsKey(gameName))
                        {
                            gameNameCounter[gameName]++;
                            gameName += $" - {gameNameCounter[gameName]}";
                        }
                        else
                        {
                            gameNameCounter.Add(gameName, 1);
                        }
                        return gameName;
                    }
                }
                return path;
            }
            private static RegistrySecurity getRegistrySecurity()
            {
                RegistrySecurity regSecurity = new RegistrySecurity();
                regSecurity.AddAccessRule(new RegistryAccessRule(new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null),
                    RegistryRights.FullControl, InheritanceFlags.None, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
                return regSecurity;

            }
            private static void UpdateGroupPolicy()
            {
                Process p = new Process();
                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                p.StartInfo.FileName = "cmd.exe";
                p.StartInfo.Arguments = "/C gpupdate /force";
                p.Start();
                p.WaitForExit(0);

            }
        }
    }
}
