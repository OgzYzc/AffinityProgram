using System.Security.AccessControl;
using AffinitySetter.Utility.Abstract;
using Microsoft.Win32;

namespace AffinitySetter.Utility.Concrete;

public class PriorityUtilities : IBaseUtilityService
{

    public void Create<T>(string keyPath, RegistryKeyPermissionCheck permissionCheck, RegistrySecurity registrySecurity, T value)
    {
        using (RegistryKey registryKey = Registry.LocalMachine.CreateSubKey(keyPath, permissionCheck, registrySecurity))
        {
            registryKey.SetValue("DevicePriority", value, RegistryValueKind.DWord);
        }
    }

    public void Delete(string keyPath, RegistryKeyPermissionCheck permissionCheck, RegistrySecurity registrySecurity)
    {
        using (RegistryKey registryKey = Registry.LocalMachine.CreateSubKey(keyPath, permissionCheck, registrySecurity))
        {
            registryKey.DeleteValue("DevicePriority");
        }
    }
}