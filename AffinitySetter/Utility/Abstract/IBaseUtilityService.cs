using System.Security.AccessControl;
using Microsoft.Win32;

namespace AffinitySetter.Utility.Abstract;

public interface IBaseUtilityService
{
    void Create<T>(string keyPath, RegistryKeyPermissionCheck permissionCheck, RegistrySecurity registrySecurity, T? value);
    void Delete(string keyPath, RegistryKeyPermissionCheck permissionCheck, RegistrySecurity registrySecurity);
}
