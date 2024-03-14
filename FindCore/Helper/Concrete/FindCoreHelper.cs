using FindCore.Helper.Abstract;
using FindCore.Utility.Abstract;

namespace FindCore.Helper.Concrete;
public class FindCoreHelper : IFindCoreHelperService
{
    private readonly IEventReaderService _eventReaderService;
    private readonly ICoreListerService _coreListerService;
    private readonly ICoreConverterService _coreConverterService;
    private readonly ICoreSelectorService _coreSelectorService;

    private static List<(int, int)>? CPPCList;
    private static List<(int, int)>? OrderedCPPCList;

    private static int[] CoresHex;

    public FindCoreHelper(IEventReaderService eventReaderService, ICoreListerService coreListerService, ICoreConverterService coreConverterService, ICoreSelectorService coreSelectorService)
    {
        _eventReaderService = eventReaderService;
        _coreListerService = coreListerService;
        _coreConverterService = coreConverterService;
        _coreSelectorService = coreSelectorService;

    }
    public void RunCPPC()
    {
        ReadCPPCEvent();
        ListCores();
        ConvertCores();
        SelectCores();
    }
    public void ReadCPPCEvent()
    {
        CPPCList = _eventReaderService.ReadEventViewer();
    }
    public void ListCores()
    {
        OrderedCPPCList = _coreListerService.PickCore(CPPCList);
    }
    public void ConvertCores()
    {
        CoresHex = _coreConverterService.ConvertToHex(OrderedCPPCList);
    }
    public void SelectCores()
    {
        _coreSelectorService.SelectCores(CoresHex);

    }
}
