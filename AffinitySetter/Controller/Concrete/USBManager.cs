using AffinitySetter.Controller.Abstract;
using AffinitySetter.Helper.Concrete;
using AffinitySetter.Model.Devices;
using AffinitySetter.Queries.Concrete;
using Base.Constants;
using Base.Utility.Abstract;
using FindCore.Model;

namespace AffinitySetter.Controller.Concrete;

public class USBManager : IBaseService
{
    private readonly IProcessorUtilityService _processorUtilityService;

    private readonly AffinityHelper _affinityHelper;
    private readonly MessageLimitHelper _messageLimitHelper;
    private readonly PriorityHelper _priorityHelper;

    List<USBModel> UsbDeviceInstanceIdList = new();
    public USBManager(AffinityHelper affinityHelper, MessageLimitHelper messageLimitHelper, PriorityHelper priorityHelper, IProcessorUtilityService processorUtilityService)
    {
        _affinityHelper = affinityHelper;
        _messageLimitHelper = messageLimitHelper;
        _priorityHelper = priorityHelper;

        _processorUtilityService = processorUtilityService;

        UsbDeviceInstanceIdList = new DeviceInfo<USBModel>().GetDevices(GuidClasses.UsbGuidClass, deviceId => new USBModel(deviceId));
        //UsbDeviceInstanceIdList = new DeviceInfo<USBModel>().GetDevices(GuidClasses.HidGuidClass, deviceId => new USBModel(deviceId));
    }

    public void AffinityAdd()
    {
        _affinityHelper.Add(UsbDeviceInstanceIdList, USBByteArrayModel.USBByteArray ?? (_processorUtilityService.GetProcessorInformation().IsSMTEnabled ? [16] : [04]));

    }
    public void AffinityDelete()
    {
        _affinityHelper.Delete(UsbDeviceInstanceIdList);
    }

    public void DeviceList()
    {
        try
        {
            foreach (var item in UsbDeviceInstanceIdList)
            {
                Console.WriteLine(item.DeviceID);
            }
        }
        catch (Exception)
        {

            throw;
        }

    }

    public void MessageLimitAdd()
    {
        _messageLimitHelper.Add(UsbDeviceInstanceIdList, 1);
    }

    public void MessageLimitDelete()
    {
        _messageLimitHelper.Delete(UsbDeviceInstanceIdList);
    }

    public void PriorityAdd()
    {
        _priorityHelper.Add(UsbDeviceInstanceIdList, 2);
    }

    public void PriorityDelete()
    {
        _priorityHelper.Delete(UsbDeviceInstanceIdList);
    }
}
