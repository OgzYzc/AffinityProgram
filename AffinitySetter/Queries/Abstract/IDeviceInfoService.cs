namespace AffinitySetter.Queries.Abstract;

internal interface IDeviceInfoService<T>
{
    List<T> GetDevices(Guid guid, Func<string, T> converter);
}
