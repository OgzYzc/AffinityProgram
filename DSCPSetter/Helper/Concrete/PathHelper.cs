using Base.Constants;
using DSCPSetter.Helper.Abstract;
using DSCPSetter.Utility.Concrete;

namespace DSCPSetter.Helper.Concrete;
public class PathHelper : IPathHelperService
{
    public string GetGameNameFromPath(string path)
    {
        int commonIndex = path.IndexOf("common", StringComparison.OrdinalIgnoreCase);

        if (commonIndex >= 0)
        {
            string substringAfterCommon = path.Substring(commonIndex + "common".Length + 1);

            int nextBackslashIndex = substringAfterCommon.IndexOf(Path.DirectorySeparatorChar);

            if (nextBackslashIndex >= 0)
            {
                string gameName = substringAfterCommon.Substring(0, nextBackslashIndex);

                if (DSCPRegistryUtility.gameNameCounter.ContainsKey(gameName))
                {
                    DSCPRegistryUtility.gameNameCounter[gameName]++;
                    gameName += $" - {DSCPRegistryUtility.gameNameCounter[gameName]}";
                }
                else
                {
                    DSCPRegistryUtility.gameNameCounter.Add(gameName, 1);
                }
                return gameName;
            }
        }
        return path;
    }

    public string GetSteamPath()
    {
        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
        {
            var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(RegistryPaths.RegistryPathMappings[6]);

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
