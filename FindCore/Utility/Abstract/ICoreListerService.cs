namespace FindCore.Utility.Abstract;

public interface ICoreListerService
{
    List<(int, int)> PickCore(List<(int, int)> eventList);
}
