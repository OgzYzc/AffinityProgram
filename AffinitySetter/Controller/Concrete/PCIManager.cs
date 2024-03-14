using AffinitySetter.Controller.Abstract;
using AffinitySetter.Helper.Concrete;
using AffinitySetter.Model.Devices;
using AffinitySetter.Queries.Concrete;
using Base.Constants;
using Base.Utility.Abstract;
using FindCore.Model;

namespace AffinitySetter.Controller.Concrete;

public class PCIManager : IBaseService
{
    private readonly IProcessorUtilityService _processorUtilityService;

    private readonly AffinityHelper _affinityHelper;
    private readonly MessageLimitHelper _messageLimitHelper;
    private readonly PriorityHelper _priorityHelper;

    List<PCIModel> PciDeviceInstanceIdList = new();

    public PCIManager(AffinityHelper affinityHelper, MessageLimitHelper messageLimitHelper, PriorityHelper priorityHelper, IProcessorUtilityService processorUtilityService)
    {
        _affinityHelper = affinityHelper;
        _messageLimitHelper = messageLimitHelper;
        _priorityHelper = priorityHelper;

        _processorUtilityService = processorUtilityService;

        PciDeviceInstanceIdList = new DeviceInfo<PCIModel>().GetDevices(GuidClasses.PciGuidClass, deviceId => new PCIModel(deviceId));
    }

    public void AffinityAdd()
    {
        _affinityHelper.Add(PciDeviceInstanceIdList, PCIByteArrayModel.PCIByteArray ?? (_processorUtilityService.GetProcessorInformation().IsSMTEnabled ? [00, 04] : [04]));
    }

    public void AffinityDelete()
    {
        _affinityHelper.Delete(PciDeviceInstanceIdList);
    }

    public void DeviceList()
    {

        foreach (var item in PciDeviceInstanceIdList)
        {
            Console.WriteLine(item.DeviceID);
        }
    }

    public void MessageLimitAdd()
    {
        _messageLimitHelper.Add(PciDeviceInstanceIdList, 1);
    }

    public void MessageLimitDelete()
    {
        _messageLimitHelper.Delete(PciDeviceInstanceIdList);
    }

    public void PriorityAdd()
    {
        _priorityHelper.Add(PciDeviceInstanceIdList, 3);
    }

    public void PriorityDelete()
    {
        _priorityHelper.Delete(PciDeviceInstanceIdList);
    }
}
