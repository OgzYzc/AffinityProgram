using System.Security.AccessControl;
using Base.Constants;
using DSCPSetter.Configuration;
using DSCPSetter.Helper.Abstract;
using DSCPSetter.Helper.Concrete;
using DSCPSetter.Model;
using DSCPSetter.Utility.Abstract;
using Microsoft.Win32;
using Windows.Graphics;

namespace DSCPSetter.Utility.Concrete
{
    public class GPUPreferenceUtility : IGPUPreferenceUtilityService
    {
        IPathHelperService _pathHelperService;
        GPUModel _gpuModel = new();

        public static Dictionary<string, int> gameNameCounter = new Dictionary<string, int>();
        public GPUPreferenceUtility(IPathHelperService pathHelperService)
        {
            _pathHelperService = pathHelperService;            
        }

        public void Create(string keyPath, RegistryKeyPermissionCheck permissionCheck, RegistrySecurity registrySecurity)
        {
            foreach (var item in ReadJsonHelper.gamePaths)
            {
                using (RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(keyPath, permissionCheck, registrySecurity))
                {
                    foreach (KeyValuePair<string, string> entry in new GPUPreferenceconfiguration().GPUPreferenceValues)
                    {
                        registryKey.SetValue(item,entry.Key+_gpuModel.AdapterID+entry.Value, RegistryValueKind.String);
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
                _gpuModel.AdapterID = registryKey.GetValue("DirectXUserGlobalSettings").ToString().Substring(16);
            }
        }
    }
}
