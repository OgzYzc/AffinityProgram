using DSCPSetter.Helper.Abstract;
using System.IO;
using System.IO.Pipes;
using System.Text.Json;

namespace DSCPSetter.Helper.Concrete;
public class ReadJsonHelper : IReadJsonHelperService
{
    private string installdirValue;
    public static HashSet<string[]> gameInstalldir;
    public static HashSet<string> gamePaths = new HashSet<string>();

    public void ReadAppInfo(string fileName)
    {
        ReadOnlySpan<byte> jsonByte = File.ReadAllBytes(fileName);
        ReadJSON(jsonByte);

    }
    public void ReadJSON(ReadOnlySpan<byte> json)
    {
        gameInstalldir = new HashSet<string[]>();
        Utf8JsonReader reader = new Utf8JsonReader(json);

        while (reader.Read())
        {
            JsonTokenType tokenType = reader.TokenType;

            switch (tokenType)
            {
                case JsonTokenType.PropertyName:
                    string propertyName = reader.GetString();

                    // Move to the next token (value)
                    if (!reader.Read())
                        continue; // If there is no value, skip to the next iteration

                    switch (propertyName)
                    {
                        case "installdir":
                            // Check if the token is a string
                            if (reader.TokenType == JsonTokenType.String)
                            {
                                installdirValue = reader.GetString();
                            }
                            break;
                        case "executable":
                            // Check if the token is a string
                            if (reader.TokenType == JsonTokenType.String)
                            {
                                string executableValue = reader.GetString();
                                if (executableValue.EndsWith(".exe"))
                                {
                                    string combination = $"{installdirValue}: {executableValue}";
                                    if (!ContainsExcludedString(combination))
                                    {
                                        gameInstalldir.Add(new string[] { installdirValue, executableValue });
                                    }
                                }
                            }
                            break;
                        default:
                            break;
                    }
                    break;
            }
        }
        File.Delete(Path.Combine(Path.GetTempPath(), "appinfo.json"));
    }


    public void ReadLibraryFolder(string fileName)
    {
        try
        {
            string[] lines = File.ReadAllLines(fileName);

            string[] paths = lines
                .Where(line => line.Contains("path"))
                .Select(line => line.Replace("\"", "").Replace(@"\\", @"\").Replace("path", "").TrimStart())
                .ToArray();

            foreach (string line in paths)
            {
                GetFolderNames(Path.Combine(line, "steamapps", "common"));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    private static void GetFolderNames(string path)
    {
        try
        {
            if (Directory.Exists(path))
            {
                string[] folders = Directory.GetDirectories(path);

                foreach (var item in folders)
                {
                    string folderName = Path.GetFileName(item);

                    foreach (var pair in gameInstalldir)
                    {
                        //if installdir from ReadJsonAppInfo is equal to folderName
                        if (pair[0] == folderName)
                        {
                            gamePaths.Add(path + @"\" + folderName + @"\" + pair[1]);
                        }
                    }
                }
            }
            else
            {
                return;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
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
}
