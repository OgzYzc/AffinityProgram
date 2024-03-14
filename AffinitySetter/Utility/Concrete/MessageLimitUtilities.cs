using AffinitySetter.Utility.Abstract;
using Microsoft.Win32;
using System.Security.AccessControl;

namespace AffinitySetter.Utility.Concrete;

public class MessageLimitUtilities : IBaseUtilityService
{

    public void Create<T>(string keyPath, RegistryKeyPermissionCheck permissionCheck, RegistrySecurity registrySecurity, T value)
    {
        using (RegistryKey registryKey = Registry.LocalMachine.CreateSubKey(keyPath, permissionCheck, registrySecurity))
        {
            registryKey.SetValue("MessageNumberLimit", value, RegistryValueKind.DWord);
        }
    }

    public void Delete(string keyPath, RegistryKeyPermissionCheck permissionCheck, RegistrySecurity registrySecurity)
    {
        using (RegistryKey registryKey = Registry.LocalMachine.CreateSubKey(keyPath, permissionCheck, registrySecurity))
        {
            registryKey.DeleteValue("MessageNumberLimit");
        }
    }
}