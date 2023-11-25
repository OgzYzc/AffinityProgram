using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AddDSCP
{
    internal class ReadJsonAppInfo
    {
        public class Read
        {
            private static string installdirValue;
            private static bool isInLaunch = false;
            private static HashSet<string> uniqueCombinations = new HashSet<string>();
            static public HashSet<string[]> gameInstalldir;
            public static void ReadAppInfo(string fileName)
            {
                ReadOnlySpan<byte> jsonReadOnlySpan = File.ReadAllBytes(fileName);
                var reader = new Utf8JsonReader(jsonReadOnlySpan);
                gameInstalldir = new HashSet<string[]>();
                while (reader.Read())
                {
                    JsonTokenType tokenType = reader.TokenType;


                    switch (tokenType)
                    {
                        case JsonTokenType.PropertyName:
                            string propertyName = reader.GetString();

                            if (propertyName == "installdir")
                            {
                                reader.Read();
                                installdirValue = reader.GetString();
                            }
                            else if (propertyName == "launch")
                            {
                                isInLaunch = true;
                            }
                            else if (isInLaunch && propertyName == "executable")
                            {
                                reader.Read();
                                string executableValue = reader.GetString();

                                while (reader.Read())
                                {
                                    if (reader.TokenType == JsonTokenType.PropertyName && reader.GetString() == "config")
                                    {
                                        reader.Read();
                                        while (reader.Read())
                                        {
                                            if (reader.TokenType == JsonTokenType.PropertyName && reader.GetString() == "oslist")
                                            {
                                                reader.Read();

                                                if (reader.TokenType == JsonTokenType.String && reader.GetString() == "windows")
                                                {
                                                    string combination = $"{installdirValue}: {executableValue}";
                                                    if (uniqueCombinations.Add(combination) && !ContainsExcludedString(combination))
                                                    {
                                                        gameInstalldir.Add(new string[] { installdirValue, executableValue });
                                                    }
                                                }
                                            }
                                            else if (reader.TokenType == JsonTokenType.EndObject)
                                            {
                                                break;
                                            }
                                        }
                                    }
                                    else if (reader.TokenType == JsonTokenType.EndObject)
                                    {
                                        break;
                                    }
                                }
                            }
                            break;

                        case JsonTokenType.EndObject:                            
                            if (isInLaunch)
                            {
                                isInLaunch = false;
                            }
                            break;
                    }
                }

                bool ContainsExcludedString(string value)
                {
                    string[] excludedStrings = {
                        "Demo", "SDK", "Server",
                        "test", "link2ea", "Beta",
                        "sdklauncher","ds","SteamWorks",
                        "bat","DO NOT USE","vbs",
                        "app","TwistedDraw","htm"};

                    return excludedStrings.Any(s => value.IndexOf(s, StringComparison.OrdinalIgnoreCase) != -1);
                }
                File.Delete(Path.Combine(Path.GetTempPath(), "appinfo.json"));
                ReadJsonLibraryFolder.Read.ReadLibraryFolder(Path.Combine(PathManager.GetSteamPath(), "steamapps", "libraryfolders.vdf"));

            }
        }
    }
}
