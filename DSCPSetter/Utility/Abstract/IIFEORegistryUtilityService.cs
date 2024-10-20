using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace DSCPSetter.Utility.Abstract;

public interface IIFEORegistryUtilityService
{
    void Create(string keyPath, RegistryKeyPermissionCheck permissionCheck, RegistrySecurity registrySecurity);
    void Delete(string keyPath, RegistryKeyPermissionCheck permissionCheck, RegistrySecurity registrySecurity);
}

