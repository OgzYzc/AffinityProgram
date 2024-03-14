namespace AffinitySetter.Controller.Abstract;

public interface IBaseService : IAffinityService, IMessageLimitService, IPriorityService
{
    void DeviceList();

}
