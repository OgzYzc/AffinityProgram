using AffinitySetter.Utility.Abstract;using Base.Constants;
using Base.Utility.Abstract;
using Microsoft.Win32;

namespace AffinitySetter.Helper.Concrete;

public class NICSettingsHelper
{
    private readonly IRegistryUtilityService _registryUtilityService;
    private readonly IProcessorUtilityService _processorUtilityService;

    public NICSettingsHelper(IRegistryUtilityService registryUtilityService, IProcessorUtilityService processorUtilityService)
    {
        _registryUtilityService = registryUtilityService;
        _processorUtilityService = processorUtilityService;

    }

    public void AddNICSettings()
    {
        ConfigureAdapterRegistrySettings();
        ConfigureNdisServiceSettings();
        ConfigureTcpipServiceSettings();
    }

    private void ConfigureAdapterRegistrySettings()
    {
        _registryUtilityService.AdapterRegistrySettings(RegistryPaths.RegistryPathMappings[3],
            RegistryKeyPermissionCheck.ReadWriteSubTree, _registryUtilityService.CreateRegistrySecurity());
    }

    private void ConfigureNdisServiceSettings()
    {
        if(FindCore.Helper.Concrete.FindCoreHelper.isFindCoreWorked=true)
        _registryUtilityService.NdisServiceSettings(RegistryPaths.RegistryPathMappings[4],
            RegistryKeyPermissionCheck.ReadWriteSubTree, _registryUtilityService.CreateRegistrySecurity(),
            FindCore.Utility.Concrete.CoreSelector.FindCoreNIC, 2);
        else
            _registryUtilityService.NdisServiceSettings(RegistryPaths.RegistryPathMappings[4],
            RegistryKeyPermissionCheck.ReadWriteSubTree, _registryUtilityService.CreateRegistrySecurity(),
            (_processorUtilityService.GetProcessorInformation().IsSMTEnabled ? 6 : 3)
            , 2);
    }

    private void ConfigureTcpipServiceSettings()
    {
        _registryUtilityService.TcpipServiceSettings(RegistryPaths.RegistryPathMappings[5],
            RegistryKeyPermissionCheck.ReadWriteSubTree, _registryUtilityService.CreateRegistrySecurity());
    }
}
