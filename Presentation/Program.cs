using AffinitySetter.Controller.Concrete;
using AffinitySetter.Helper.Concrete;
using AffinitySetter.Utility.Abstract;
using AffinitySetter.Utility.Concrete;
using Base.Utility;
using Base.Utility.Abstract;
using Base.Utility.Concrete;
using DSCPSetter.Helper.Abstract;
using DSCPSetter.Helper.Concrete;
using DSCPSetter.Utility.Abstract;
using DSCPSetter.Utility.Concrete;
using FindCore.Helper.Abstract;
using FindCore.Helper.Concrete;
using FindCore.Utility.Abstract;
using FindCore.Utility.Concrete;
using Presentation.View.Controller.Concrete;
using Presentation.View.Helper.Concrete;
using Presentation.View.Utility.Abstract;
using Presentation.View.Utility.Concrete;
class Program
{
    static void Main(string[] args)
    {
        // Utilities
        Lazy<IProcessorUtilityService> processorUtility = new Lazy<IProcessorUtilityService>(() => new ProcessorUtilities());
        Lazy<IBaseUtilityService> affinityUtility = new Lazy<IBaseUtilityService>(() => new AffinityUtilities());
        Lazy<IBaseUtilityService> messageLimitUtility = new Lazy<IBaseUtilityService>(() => new MessageLimitUtilities());
        Lazy<IBaseUtilityService> priorityUtility = new Lazy<IBaseUtilityService>(() => new PriorityUtilities());
        Lazy<ICommandLineUtilityService> commandLineUtility = new Lazy<ICommandLineUtilityService>(() => new CommandLineUtility());
        Lazy<IRegistryUtilityService> registryUtility = new Lazy<IRegistryUtilityService>(() => new RegistryUtilities(processorUtility.Value, commandLineUtility.Value));

        // Helpers 
        Lazy<AffinityHelper> affinityHelper = new Lazy<AffinityHelper>(() => new AffinityHelper(affinityUtility.Value, registryUtility.Value));
        Lazy<MessageLimitHelper> messageLimitHelper = new Lazy<MessageLimitHelper>(() => new MessageLimitHelper(messageLimitUtility.Value, registryUtility.Value));
        Lazy<PriorityHelper> priorityHelper = new Lazy<PriorityHelper>(() => new PriorityHelper(priorityUtility.Value, registryUtility.Value));
        Lazy<NICSettingsHelper> nicSettingsHelper = new Lazy<NICSettingsHelper>(() => new NICSettingsHelper(registryUtility.Value, processorUtility.Value));

        // Managers 
        Lazy<NICManager> nicManager = new Lazy<NICManager>(() => new NICManager(affinityHelper.Value, messageLimitHelper.Value, priorityHelper.Value, nicSettingsHelper.Value, processorUtility.Value));
        Lazy<PCIManager> pciManager = new Lazy<PCIManager>(() => new PCIManager(affinityHelper.Value, messageLimitHelper.Value, priorityHelper.Value, processorUtility.Value));
        Lazy<USBManager> usbManager = new Lazy<USBManager>(() => new USBManager(affinityHelper.Value, messageLimitHelper.Value, priorityHelper.Value, processorUtility.Value));


        // FindCore 
        Lazy<IEventReaderService> eventReaderService = new Lazy<IEventReaderService>(() => new EventReader(processorUtility.Value));
        Lazy<ICoreListerService> coreListerService = new Lazy<ICoreListerService>(() => new CoreLister());
        Lazy<ICoreConverterService> coreConverterService = new Lazy<ICoreConverterService>(() => new CoreConverter(processorUtility.Value));
        Lazy<ICoreSelectorService> coreSelectorService = new Lazy<ICoreSelectorService>(() => new CoreSelector(coreConverterService.Value));
        Lazy<IFindCoreHelperService> findCoreHelperService = new Lazy<IFindCoreHelperService>(() => new FindCoreHelper(eventReaderService.Value, coreListerService.Value, coreConverterService.Value, coreSelectorService.Value));

        // DSCP 
        Lazy<IDSCPMiscUtilityService> dscpMiscUtilityService = new Lazy<IDSCPMiscUtilityService>(() => new DSCPMiscUtility(commandLineUtility.Value));
        Lazy<IPathHelperService> pathHelperService = new Lazy<IPathHelperService>(() => new PathHelper());
        Lazy<IVDFConverterUtilityService> vdfConverterUtilityService = new Lazy<IVDFConverterUtilityService>(() => new VDFConverterUtility(pathHelperService.Value));
        Lazy<IReadJsonHelperService> readJsonHelperService = new Lazy<IReadJsonHelperService>(() => new ReadJsonHelper());
        Lazy<IDSCPRegistryUtilityService> dscpRegistryUtilityService = new Lazy<IDSCPRegistryUtilityService>(() => new DSCPRegistryUtility(pathHelperService.Value));
        Lazy<IIFEORegistryUtilityService> ifeoRegistryUtilityService = new Lazy<IIFEORegistryUtilityService>(() => new IFEORegistryUtility(pathHelperService.Value));
        Lazy<IDSCPSetterHelper> dscpSetterHelper = new Lazy<IDSCPSetterHelper>(() => new DSCPSetterHelper(dscpMiscUtilityService.Value, vdfConverterUtilityService.Value, pathHelperService.Value, readJsonHelperService.Value, dscpRegistryUtilityService.Value, ifeoRegistryUtilityService.Value, registryUtility.Value));

        Lazy<IDisplayMenuUtilityService> displayMenuUtilityService = new Lazy<IDisplayMenuUtilityService>(() => new DisplayMenuUtility(processorUtility.Value, findCoreHelperService.Value, dscpSetterHelper.Value, nicManager.Value, pciManager.Value, usbManager.Value));
        Lazy<IDisplaySetupUtilityService> displaySetupUtilityService = new Lazy<IDisplaySetupUtilityService>(() => new DisplaySetupUtility());

        // Main menu
        Lazy<DisplayMenuHelper> displayMenuHelper = new Lazy<DisplayMenuHelper>(() => new DisplayMenuHelper(displayMenuUtilityService.Value));
        Lazy<DisplaySetupHelper> displaySetupHelper = new Lazy<DisplaySetupHelper>(() => new DisplaySetupHelper(displaySetupUtilityService.Value));


        Lazy<UserInterfaceManager> userInterfaceManager = new Lazy<UserInterfaceManager>(() => new UserInterfaceManager(displayMenuHelper.Value, displaySetupHelper.Value));
        userInterfaceManager.Value.DisplayInterface();
    }
}
