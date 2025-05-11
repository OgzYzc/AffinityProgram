namespace Base.Constants;

public class RegistryPaths
{
    public static readonly Dictionary<int, string> RegistryPathMappings = new Dictionary<int, string>
    {
        { 1, @"SYSTEM\CurrentControlSet\Enum\$i\Device Parameters\Interrupt Management\Affinity Policy" },                      //AffinityPolicyRegistryPath
        { 2, @"SYSTEM\CurrentControlSet\Enum\$i\Device Parameters\Interrupt Management\MessageSignaledInterruptProperties" },   //MessageLimitRegistryPath
        { 3, @"SYSTEM\CurrentControlSet\Control\Class\{4d36e972-e325-11ce-bfc1-08002be10318}\" },                               //AdapterRegistryPath
        { 4, @"SYSTEM\CurrentControlSet\Services\NDIS\Parameters" },                                                            //NdisServiceRegistryPath
        { 5, @"SYSTEM\CurrentControlSet\Services\Tcpip" },                                                                      //TcpipServiceRegistryPath        
        { 6, @"SOFTWARE\Valve\Steam" },                                                                                         //SteamPath(HKCU)
        { 7, @"SOFTWARE\Policies\Microsoft\Windows\QoS" },                                                                      //QoSPath
        //{ 8, @"SOFTWARE\Microsoft\Windows\CurrentVersion\Group Policy Objects" }                                              //GPEDITPath(HKCU)
        { 9, @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options" },                                    //IFEOPath
        { 10, @"Software\Microsoft\DirectX\UserGpuPreferences" },                                                               //GPUPreferencePath(HKCU)
    };
}
