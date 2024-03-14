using Base.Model;
using Base.Utility.Abstract;

namespace AffinitySetter.Helper.Concrete;
internal class ProcessorHelper
{
    private readonly IProcessorUtilityService _processorUtilityService;
    public ProcessorHelper(IProcessorUtilityService processorUtilityService)
    {
        _processorUtilityService = processorUtilityService;
    }
    public ProcessorModel.ProcessorInfoModel GetProcessorInformation()
    {
        return _processorUtilityService.GetProcessorInformation();
    }
}
