using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace AddDSCP
{
    internal class ReadJsonAppInfo
    {
        public class Read
        {
            private static string installdirValue;
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
                            else if (propertyName == "executable")
                            {
                                reader.Read();
                                string executableValue = reader.GetString();
                                if (executableValue.EndsWith(".exe"))
                                {
                                    string combination = $"{installdirValue}: {executableValue}";
                                    if (uniqueCombinations.Add(combination) && !ContainsExcludedString(combination))
                                    {
                                        gameInstalldir.Add(new string[] { installdirValue, executableValue });
                                    }
                                }
                            }
                            break;
                    }
                }

                bool ContainsExcludedString(string value)
                {
                    string[] excludedStrings = {
                        "Demo", "SDK", "Server",
                        "test", "link2ea", "Beta",
                        "sdklauncher","SteamWorks",
                        "bat","DO NOT USE","vbs",
                        "app","TwistedDraw","htm","Artwork"};
                    return excludedStrings.Any(s => value.IndexOf(s, StringComparison.OrdinalIgnoreCase) != -1);
                }
                File.Delete(Path.Combine(Path.GetTempPath(), "appinfo.json"));
                ReadJsonLibraryFolder.Read.ReadLibraryFolder(Path.Combine(PathManager.GetSteamPath(), "steamapps", "libraryfolders.vdf"));

            }
        }
    }
}
