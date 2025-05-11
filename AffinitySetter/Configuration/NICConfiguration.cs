using Microsoft.Win32;

namespace AffinitySetter.Configuration;

internal class NICConfiguration
{
    internal Dictionary<string, string> smtRssValues = new Dictionary<string, string>()
    {
        { "*RSS", "1" },
        { "*RSSProfile", "4" },
        { "*RssBaseProcNumber", "6" },
        { "*MaxRssProcessors", "2" },
        { "*NumaNodeId", "0" },
        { "*NumRssQueues", "2" },
        { "*RssBaseProcGroup", "0" },
        { "*RssMaxProcNumber", "8" },
        { "*RssMaxProcGroup", "0" },
    };

    internal Dictionary<string, string> nonSmtRssValues = new Dictionary<string, string>()
    {
        { "*RSS", "1" },
        { "*RSSProfile", "4" },
        { "*RssBaseProcNumber", "3" },
        { "*MaxRssProcessors", "2" },
        { "*NumaNodeId", "0" },
        { "*NumRssQueues", "2" },
        { "*RssBaseProcGroup", "0" },
        { "*RssMaxProcNumber", "4" },
        { "*RssMaxProcGroup", "0" },
    };

    internal Dictionary<string, (string Value, RegistryValueKind Type)> NicValues = new Dictionary<string, (string, RegistryValueKind)>()
    {
         // Latency
        { "*FlowControl",("0" ,RegistryValueKind.String)},
        { "*InterruptModeration",("0" ,RegistryValueKind.String)},
        { "*LsoV1IPv4",("0" ,RegistryValueKind.String)},
        { "*LsoV2IPv4",("0" ,RegistryValueKind.String)},
        { "*LsoV2IPv6",("0" ,RegistryValueKind.String)},
        { "*PMNSOffload",("1" ,RegistryValueKind.String)},
        { "*PMARPOffload",("1" ,RegistryValueKind.String)},
        { "*PriorityVLANTag",("1",RegistryValueKind.String )},          // Enabling packet priority for QoS
        { "*PacketDirect",("1" ,RegistryValueKind.String)},
        { "*RscIPv4",("0" ,RegistryValueKind.String)},
        { "*RscIPv6",("0" ,RegistryValueKind.String)},
        { "*ReceiveBuffers",("2048" ,RegistryValueKind.String)},
        { "*TransmitBuffers",("2048" ,RegistryValueKind.String)},
        { "CoalesceBufferSize",("0",RegistryValueKind.String )},        // Coalescing
        { "DMACoalescing",("0",RegistryValueKind.String )},             // Coalescing
        { "EnableCoalesce",("0" ,RegistryValueKind.DWord)},             // Coalescing
        //{ "DcaRxSettings",("" },                                      // Dont know value
        //{ "DcaTxSettings",("" },                                      // Dont know value
        { "EnableLLI",("2" ,RegistryValueKind.String)},                 // TCP PSH flag
        { "EnableDCA",("1" ,RegistryValueKind.String)},
        { "EnableUdpTxScaling",("1" ,RegistryValueKind.DWord)},
        { "ManyCoreScaling",("1" ,RegistryValueKind.DWord)},
        { "ITR",("0" ,RegistryValueKind.String)},
        { "ReceiveScalingMode",("0",RegistryValueKind.String)},         // Disabling this fills up indirection table
        { "TxIntDelay",("0" ,RegistryValueKind.String)},                // This settings supposed to do nothing because we already disabled interrupt moderation but adding here just in case
        // These ones are listed in intel linux driver documentation. Not sure it works.
        // downloadmirror.intel.com/15817/eng/readme.txt
        { "RxIntDelay",("0" ,RegistryValueKind.String)},                // This settings supposed to do nothing because we already disabled interrupt moderation but adding here just in case
        { "RxAbsIntDelay",("0" ,RegistryValueKind.String)},             // This settings supposed to do nothing because we already disabled interrupt moderation but adding here just in case
        { "TxAbsIntDelay",("0" ,RegistryValueKind.String)},             // This settings supposed to do nothing because we already disabled interrupt moderation but adding here just in case
        // Power saving
        { "*EEE",("0" ,RegistryValueKind.String)},
        { "EnablePME",("0" ,RegistryValueKind.String)},
        { "EnableModernStandby",("0" ,RegistryValueKind.String)},
        { "EEELinkAdvertisement",("0" ,RegistryValueKind.String)},
        { "DeviceSleepOnDisconnect",("0" ,RegistryValueKind.String)},
        { "*EnableDynamicPowerGating",("0" ,RegistryValueKind.String)},
        { "*NicAutoPowerSaver",("0" ,RegistryValueKind.String)},
        { "AutoPowerSaveModeEnabled",("0" ,RegistryValueKind.DWord)},
        { "DisableDelayedPowerUp",("1" ,RegistryValueKind.DWord)},
        { "EnableConnectedPowerGating",("0",RegistryValueKind.DWord)},
        { "EnablePowerManagement",("0" ,RegistryValueKind.String)},
        { "EnableSavePowerNow",("0" ,RegistryValueKind.String)},
        { "ReduceSpeedOnPowerDown",("0" ,RegistryValueKind.String)},
        { "PnPCapabilities",("24" ,RegistryValueKind.DWord)},
        { "*SelectiveSuspend",("0" ,RegistryValueKind.String)},
        { "MediaDisconnectToSleepTimeOut",("0" ,RegistryValueKind.String)},
        { "*WakeOnMagicPacket",("0" ,RegistryValueKind.String)},
        { "*WakeOnPattern",("0" ,RegistryValueKind.String)},
        { "*ModernStandbyWoLMagicPacket",("0" ,RegistryValueKind.String)},
        { "WakeOnLink",("0" ,RegistryValueKind.String)},
        { "WaitAutoNegComplete",("0" ,RegistryValueKind.String)},
        { "*SpeedDuplex",("5" ,RegistryValueKind.String)},
    };

    // These ones have their keys in registry but needs test
    //{ "AdaptiveIFS",("0" },
    //{ "AIMLowestLatency",("1" },
    //{ "DynamicLTR",("0" },
    //{ "EnableAdvancedDynamicITR",("0" },
    //{ "EnableD0PHYFlexibleSpeed",("0" },
    //{ "EnabledDatapathCycleCounters",("0    " },
    //{ "EnabledDatapathEventCounters",("0" },
    //{ "EnableDeviceBusPowerStateDependency",("0" },
    //{ "EnableDisconnectedStandby",("0" },
    //{ "EnableDRBT",("1" },
    //{ "EnableEIAM",("1" },
    //{ "EnableETW",("1" },
    //{ "EnableExtraLinkUpRetries",("0" },
    //{ "EnableHWAutonomous",("0" },
    //{ "EnableIAM",("1" },
    //{ "EnableLHRssWA",("1" },
    //{ "EnableLocklessTx",("1" },
    //{ "EnablePHYFlexibleSpeed",("1" },
    //{ "EnablePHYWakeUp",("0" },
    //{ "EnableRxDescriptorChaining",("1" },
    //{ "EnableTcpTimer",("1" },
    //{ "EnableTss",("1" },
    //{ "EnableTxHangWA",("1" },
    //{ "EnableTxHeadWB",("1" },
    //{ "EnableWakeOnManagmentOnTCO",("0" },
    //{ "ManyCoreScaling",("1" },
}
