using FindCore.Utility.Abstract;

namespace FindCore.Utility.Concrete;

public class CoreLister : ICoreListerService
{
    public List<(int, int)> PickCore(List<(int, int)> eventList)
    {
        var maxPerformanceProcessorList = eventList.OrderByDescending(pair => pair.Item2).ToList();

        Console.WriteLine("\n" + "Processor order:");
        Console.WriteLine(string.Join(" > ", maxPerformanceProcessorList.Select(pair => $"Processor {pair.Item1}")));

        return maxPerformanceProcessorList;
    }
}