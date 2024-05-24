using Microsoft.Win32;
using System.Security.AccessControl;

namespace AffinitySetter.Utility.Abstract;

public interface IRegistryUtilityService
{
    RegistrySecurity CreateRegistrySecurity();

    void AdapterRegistrySettings(string keyPath, RegistryKeyPermissionCheck permissionCheck, RegistrySecurity registrySecurity);
    void NdisServiceSettings(string keyPath, RegistryKeyPermissionCheck permissionCheck, RegistrySecurity registrySecurity, int baseCpuNumber, int maxCpuNumber);
    void TcpipServiceSettings(string keyPath, RegistryKeyPermissionCheck permissionCheck, RegistrySecurity registrySecurity);
    void RSSRegistrySettings(string? response, string keyPath, RegistryKeyPermissionCheck permissionCheck, RegistrySecurity registrySecurity);
    void SetAdapterState(string state);
}
