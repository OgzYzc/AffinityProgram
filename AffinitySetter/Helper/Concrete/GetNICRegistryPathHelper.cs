using Microsoft.Win32;

namespace AffinitySetter.Helper.Concrete;

internal class GetNICRegistryPathHelper
{
    internal string? GetDriverPath(string regPath)
    {
        List<string> ProviderList = new List<string> {
        "Intel",
        "Realtek",
        "Marvell",
        "Mellanox"
        };

        foreach (var Provider in ProviderList)
        {
            try
            {
                using (RegistryKey? classKey = Registry.LocalMachine.OpenSubKey(regPath))
                {
                    foreach (string subKeyName in from subKeyName in classKey.GetSubKeyNames()
                                                  where subKeyName != "Configuration" && subKeyName != "Properties"
                                                  select subKeyName)
                    {
                        using (RegistryKey? productKey = classKey.OpenSubKey(subKeyName))
                        {
                            string? keyValue = productKey?.GetValue("ProviderName")?.ToString();
                            if (keyValue != null && keyValue.Equals(Provider.ToString(), StringComparison.OrdinalIgnoreCase))
                            {
                                return subKeyName;
                            }
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }
        return null;
    }
}
