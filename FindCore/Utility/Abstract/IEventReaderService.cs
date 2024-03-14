namespace FindCore.Utility.Abstract;

public interface IEventReaderService
{
    List<(int, int)> ReadEventViewer();
}
