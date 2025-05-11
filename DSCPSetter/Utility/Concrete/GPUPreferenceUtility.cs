using System.Security.AccessControl;
using AffinitySetter.Model.Devices;
using Base.Constants;
using DSCPSetter.Configuration;
using DSCPSetter.Helper.Abstract;
using DSCPSetter.Helper.Concrete;
using DSCPSetter.Model;
using DSCPSetter.Utility.Abstract;
using Microsoft.Win32;

namespace DSCPSetter.Utility.Concrete
{
    public class GPUPreferenceUtility : IGPUPreferenceUtilityService
    {
        IPathHelperService _pathHelperService;
        GPUModel _gpuModel = new();
        List<PCIModel> PciDeviceInstanceIdList = new();

        public static Dictionary<string, int> gameNameCounter = new Dictionary<string, int>();
        public GPUPreferenceUtility(IPathHelperService pathHelperService)
        {
            _pathHelperService = pathHelperService;
            PciDeviceInstanceIdList = new AffinitySetter.Queries.Concrete.DeviceInfo<PCIModel>().GetDevices(GuidClasses.PciGuidClass, deviceId => new PCIModel(deviceId));
        }

        public void Create(string keyPath, RegistryKeyPermissionCheck permissionCheck, RegistrySecurity registrySecurity)
        {
            foreach (var item in ReadJsonHelper.gamePaths)
            {
                using (RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(keyPath, permissionCheck, registrySecurity))
                {
                    foreach (KeyValuePair<string, string> entry in new GPUPreferenceconfiguration().GPUPreferenceValues)
                    {
                        if (_gpuModel.AdapterID == null) //PciDeviceInstanceIdList[0] returns external GPU
                            registryKey.SetValue(item, entry.Key + PciDeviceInstanceIdList[0].DeviceID.Substring(8, 29).Replace("DEV_", "").Replace("SUBSYS_", "").ToString() + ";" + entry.Value, RegistryValueKind.String);

                        else
                            registryKey.SetValue(item, entry.Key + _gpuModel.AdapterID + entry.Value, RegistryValueKind.String);
                    }
                }
            }
            Console.WriteLine(Messages.GPUPreferenceAdded);
        }

        public void Delete(string keyPath, RegistryKeyPermissionCheck permissionCheck, RegistrySecurity registrySecurity)
        {
            //FILL OUT LATER
        }
        public void ReadEntry(string keyPath)
        {
            using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(keyPath))
            {
                try
                {
                    _gpuModel.AdapterID = registryKey.GetValue("DirectXUserGlobalSettings").ToString()[16..];
                }
                catch (Exception)
                {
                    return;
                }
            }
        }
    }
}
