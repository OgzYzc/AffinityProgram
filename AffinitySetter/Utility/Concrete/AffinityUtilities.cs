using AffinitySetter.Utility.Abstract;
using Microsoft.Win32;
using System.Security.AccessControl;

namespace AffinitySetter.Utility.Concrete;

public class AffinityUtilities : IBaseUtilityService
{
    public void Create<T>(string keyPath, RegistryKeyPermissionCheck permissionCheck, RegistrySecurity registrySecurity, T value)
    {
        using (RegistryKey registryKey = Registry.LocalMachine.CreateSubKey(keyPath, permissionCheck, registrySecurity))
        {
            registryKey.SetValue("AssignmentSetOverride", value: value, RegistryValueKind.Binary);
            registryKey.SetValue("DevicePolicy", "4", RegistryValueKind.DWord);
        }
    }

    public void Delete(string keyPath, RegistryKeyPermissionCheck permissionCheck, RegistrySecurity registrySecurity)
    {
        using (RegistryKey registryKey = Registry.LocalMachine.CreateSubKey(keyPath, permissionCheck, registrySecurity))
        {
            registryKey.DeleteValue("AssignmentSetOverride");
            registryKey.DeleteValue("DevicePolicy");
        }
    }
}