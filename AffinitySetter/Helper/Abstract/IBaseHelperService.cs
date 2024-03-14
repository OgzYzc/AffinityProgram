using AffinitySetter.Model.Devices;

namespace AffinitySetter.Helper.Abstract;

public interface IBaseHelperService
{
    void Add<T>(List<T> deviceInstanceIdList, object value) where T : BaseModel;
    void Delete<T>(List<T> deviceInstanceIdList) where T : BaseModel;
}
