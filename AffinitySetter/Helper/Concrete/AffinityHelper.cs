using AffinitySetter.Helper.Abstract;
using AffinitySetter.Model.Devices;
using AffinitySetter.Utility.Abstract;
using Base.Constants;
using Microsoft.Win32;

namespace AffinitySetter.Helper.Concrete;

public class AffinityHelper : IBaseHelperService
{
    IBaseUtilityService _baseUtilityService;
    IRegistryUtilityService _registryUtilityService;
    public AffinityHelper(IBaseUtilityService baseUtilityService, IRegistryUtilityService registryUtilityService)
    {
        _baseUtilityService = baseUtilityService;
        _registryUtilityService = registryUtilityService;
    }

    public void Add<T>(List<T> deviceInstanceIdList, object value) where T : BaseModel
    {
        List<string> deviceIDs = new GetDeviceHelper().GetDeviceID(deviceInstanceIdList, RegistryPaths.RegistryPathMappings[1]);

        foreach (string keyPath in deviceIDs)
        {
            _baseUtilityService.Create(keyPath,
                RegistryKeyPermissionCheck.ReadWriteSubTree,
                _registryUtilityService.CreateRegistrySecurity(),
                value
                );
        }
        Console.WriteLine(Messages.AffinityAdded);
    }

    public void Delete<T>(List<T> deviceInstanceIdList) where T : BaseModel
    {
        List<string> deviceIDs = new GetDeviceHelper().GetDeviceID(deviceInstanceIdList, RegistryPaths.RegistryPathMappings[1]);

        foreach (string keyPath in deviceIDs)
        {
            _baseUtilityService.Delete(keyPath,
                RegistryKeyPermissionCheck.ReadWriteSubTree,
                _registryUtilityService.CreateRegistrySecurity()
                );
        }
        Console.WriteLine(Messages.AffinityDeleted);
    }
}
