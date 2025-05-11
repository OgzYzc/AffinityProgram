using System.Security.AccessControl;
using Microsoft.Win32;

namespace DSCPSetter.Utility.Abstract;

public interface IIFEORegistryUtilityService
{
    void Create(string keyPath, RegistryKeyPermissionCheck permissionCheck, RegistrySecurity registrySecurity);
    void Delete(string keyPath, RegistryKeyPermissionCheck permissionCheck, RegistrySecurity registrySecurity);
}

