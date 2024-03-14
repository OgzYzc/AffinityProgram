using Microsoft.Win32;
using System.Security.AccessControl;

namespace AffinitySetter.Utility.Abstract;

public interface IBaseUtilityService
{
    void Create<T>(string keyPath, RegistryKeyPermissionCheck permissionCheck, RegistrySecurity registrySecurity, T? value);
    void Delete(string keyPath, RegistryKeyPermissionCheck permissionCheck, RegistrySecurity registrySecurity);
}
