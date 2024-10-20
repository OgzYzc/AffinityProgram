using AffinitySetter.Utility.Abstract;
using Base.Constants;
using DSCPSetter.Configuration;
using DSCPSetter.Helper.Abstract;
using DSCPSetter.Helper.Concrete;
using DSCPSetter.Utility.Abstract;
using Microsoft.Win32;
using System.Security.AccessControl;

namespace DSCPSetter.Utility.Concrete;
public class DSCPRegistryUtility : IDSCPRegistryUtilityService
{
    IPathHelperService _pathHelperService;

    public static Dictionary<string, int> gameNameCounter = new Dictionary<string, int>();
    public DSCPRegistryUtility(IPathHelperService pathHelperService)
    {
        _pathHelperService = pathHelperService;
    }

    public void Create(string keyPath, RegistryKeyPermissionCheck permissionCheck, RegistrySecurity registrySecurity)
    {
        using (RegistryKey registryKey = Registry.LocalMachine.CreateSubKey(keyPath, permissionCheck, registrySecurity))
        {
            registryKey.SetValue("Application DSCP Marking Request", "Allowed", RegistryValueKind.String);

            foreach (var item in ReadJsonHelper.gamePaths)
            {
                string gameName = _pathHelperService.GetGameNameFromPath(item);

                using (RegistryKey dscpKey = registryKey.CreateSubKey(gameName))
                {
                    foreach (KeyValuePair<string, string> entry in new DSCPConfiguration().DSCPValues)
                    {
                        if (entry.Key == "Application Name")
                            dscpKey.SetValue(entry.Key, item, RegistryValueKind.String);
                        else
                            dscpKey.SetValue(entry.Key, entry.Value, RegistryValueKind.String);
                    }
                }
            }
        }
        /* Clear memory */
        GC.Collect();                   
        GC.WaitForPendingFinalizers();  

        Console.WriteLine(Messages.DSCPAdded);
    }

    public void Delete(string keyPath, RegistryKeyPermissionCheck permissionCheck, RegistrySecurity registrySecurity)
    {
        throw new NotImplementedException();
    }
}
