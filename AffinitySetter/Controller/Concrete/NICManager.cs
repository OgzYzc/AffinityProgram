using AffinitySetter.Controller.Abstract;
using AffinitySetter.Helper.Concrete;
using AffinitySetter.Model.Devices;
using AffinitySetter.Queries.Concrete;
using Base.Constants;
using Base.Utility.Abstract;
using FindCore.Model;

namespace AffinitySetter.Controller.Concrete;

public class NICManager : IBaseService
{
    private readonly IProcessorUtilityService _processorUtilityService;

    private readonly AffinityHelper _affinityHelper;
    private readonly MessageLimitHelper _messageLimitHelper;
    private readonly PriorityHelper _priorityHelper;
    private readonly NICSettingsHelper _nicSettingsHelper;

    List<NICModel> NicDeviceInstanceIdList = new();
    public NICManager(AffinityHelper affinityHelper, MessageLimitHelper messageLimitHelper, PriorityHelper priorityHelper, NICSettingsHelper nicSettingsHelper, IProcessorUtilityService processorUtilityService)
    {
        _affinityHelper = affinityHelper;
        _messageLimitHelper = messageLimitHelper;
        _priorityHelper = priorityHelper;
        _nicSettingsHelper = nicSettingsHelper;

        _processorUtilityService = processorUtilityService;

        NicDeviceInstanceIdList = new DeviceInfo<NICModel>().GetDevices(GuidClasses.NicGuidClass, deviceId => new NICModel(deviceId));

    }
    public void AffinityAdd()
    {
        _affinityHelper.Add(NicDeviceInstanceIdList, NICByteArrayModel.NICByteArray ?? (_processorUtilityService.GetProcessorInformation().IsSMTEnabled ? [00, 04] : [24]));
    }

    public void AffinityDelete()
    {
        _affinityHelper.Delete(NicDeviceInstanceIdList);
    }

    public void DeviceList()
    {
        foreach (var item in NicDeviceInstanceIdList)
        {
            Console.WriteLine(item.DeviceID);
        }

    }

    public void MessageLimitAdd()
    {
        _messageLimitHelper.Add(NicDeviceInstanceIdList, 4);
    }

    public void MessageLimitDelete()
    {
        _messageLimitHelper.Delete(NicDeviceInstanceIdList);
    }

    public void PriorityAdd()
    {
        _priorityHelper.Add(NicDeviceInstanceIdList, 2);
    }

    public void PriorityDelete()
    {
        _priorityHelper.Delete(NicDeviceInstanceIdList);
    }

    public void RegistrySettingsAdd()
    {
        _nicSettingsHelper.AddNICSettings();
    }
}
