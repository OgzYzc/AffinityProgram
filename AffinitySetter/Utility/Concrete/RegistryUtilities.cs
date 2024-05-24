using AffinitySetter.Configuration;
using AffinitySetter.Helper.Concrete;
using AffinitySetter.Utility.Abstract;
using Base.Constants;
using Base.Utility;
using Base.Utility.Abstract;
using Microsoft.Win32;
using System.Security.AccessControl;
using System.Security.Principal;

namespace AffinitySetter.Utility.Concrete;

public class RegistryUtilities : IRegistryUtilityService
{
    private readonly IProcessorUtilityService _processorUtilityService;
    private readonly ICommandLineUtilityService _commandLineUtilityService;

    public RegistryUtilities(IProcessorUtilityService processorUtilityService,ICommandLineUtilityService commandLineUtilityService)
    {
        _processorUtilityService = processorUtilityService;
        _commandLineUtilityService = commandLineUtilityService;
    }

    public RegistrySecurity CreateRegistrySecurity()
    {
        RegistrySecurity regSecurity = new RegistrySecurity();
        regSecurity.AddAccessRule(new RegistryAccessRule(new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null), RegistryRights.FullControl, InheritanceFlags.None, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
        return regSecurity;
    }

    public void AdapterRegistrySettings(string keyPath, RegistryKeyPermissionCheck permissionCheck, RegistrySecurity registrySecurity)
    {
        string adapterRegistryPath = keyPath + new GetNICRegistryPathHelper().GetDriverPath(keyPath);


        using (RegistryKey registryKey = Registry.LocalMachine.CreateSubKey(adapterRegistryPath, permissionCheck, registrySecurity))
        {
            foreach (var item in new NICConfiguration().NicValues)
                registryKey.SetValue(item.Key, item.Value.Value, item.Value.Type);


            Console.WriteLine("Do you want to add RSS values? (Type 'yes' or 'y')");
            string response = Console.ReadLine()?.ToLower();

            if (response == "yes" || response == "y")
            {
                // Disable all adapter
                string executablePath = @"C:\Windows\System32\netsh.exe";
                string arguments = "interface set interface '*' admin=disabled";
                string command = $"{executablePath} {arguments}";
                _commandLineUtilityService.StartCMD(command);



                Dictionary<string, string> selectedRssValues = _processorUtilityService.GetProcessorInformation().IsSMTEnabled ? new NICConfiguration().smtRssValues : new NICConfiguration().nonSmtRssValues;
                foreach (KeyValuePair<string, string> value in selectedRssValues)
                    registryKey.SetValue(value.Key, value.Value, RegistryValueKind.String);

                Console.WriteLine(Messages.RSSSettingsAdded);
            }
            Console.WriteLine(Messages.NICSettingsAdded);
        }

    }
    public void NdisServiceSettings(string keyPath, RegistryKeyPermissionCheck permissionCheck, RegistrySecurity registrySecurity, int baseCpuNumber, int maxCpuNumber)
    {
        using (RegistryKey registryKey = Registry.LocalMachine.CreateSubKey(keyPath, permissionCheck, registrySecurity))
        {
            registryKey.SetValue("RssBaseCpu", baseCpuNumber, RegistryValueKind.DWord);
            registryKey.SetValue("MaxNumRssCpus", maxCpuNumber, RegistryValueKind.DWord);
        }
        Console.WriteLine(Messages.NDISServiceSettingsAdded);
    }

    public void TcpipServiceSettings(string keyPath, RegistryKeyPermissionCheck permissionCheck, RegistrySecurity registrySecurity)
    {
        using (RegistryKey registryKey = Registry.LocalMachine.CreateSubKey(keyPath, permissionCheck, registrySecurity))
        {
            registryKey.SetValue("DisableTaskOffload", 0, RegistryValueKind.DWord);
        }
        Console.WriteLine(Messages.TcpIpServiceSettingsAdded);
    }
}
