﻿using System.Security.AccessControl;
using Base.Constants;
using DSCPSetter.Configuration;
using DSCPSetter.Helper.Abstract;
using DSCPSetter.Helper.Concrete;
using DSCPSetter.Utility.Abstract;
using Microsoft.Win32;

namespace DSCPSetter.Utility.Concrete
{
    public class IFEORegistryUtility : IIFEORegistryUtilityService
    {
        IPathHelperService _pathHelperService;

        public static Dictionary<string, int> gameNameCounter = new Dictionary<string, int>();
        public IFEORegistryUtility(IPathHelperService pathHelperService)
        {
            _pathHelperService = pathHelperService;
        }

        public void Create(string keyPath, RegistryKeyPermissionCheck permissionCheck, RegistrySecurity registrySecurity)
        {
            foreach (var item in ReadJsonHelper.gamePaths)
            {
                var executable = item.Split(new[] { '\\', '/' }).LastOrDefault(part => part.EndsWith(".exe", StringComparison.OrdinalIgnoreCase));
                using (RegistryKey registryKey = Registry.LocalMachine.CreateSubKey(keyPath + "\\" + executable, permissionCheck, registrySecurity))
                {
                    using (RegistryKey ifeoKey = registryKey.CreateSubKey("PerfOptions", permissionCheck, registrySecurity))
                    {
                        foreach (KeyValuePair<string, int> entry in new IFEOConfiguration().IFEOValues)
                        {
                            ifeoKey.SetValue(entry.Key, entry.Value, RegistryValueKind.DWord);
                        }
                    }
                }
            }
            /* Clear memory */
            GC.Collect();
            GC.WaitForPendingFinalizers();

            Console.WriteLine(Messages.IFEOAdded);
        }
        public void Delete(string keyPath, RegistryKeyPermissionCheck permissionCheck, RegistrySecurity registrySecurity)
        {
            //FILLOUTLATER
        }
    }
}
