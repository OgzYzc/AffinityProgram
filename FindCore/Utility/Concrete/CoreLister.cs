using FindCore.Utility.Abstract;

namespace FindCore.Utility.Concrete;

public class CoreLister : ICoreListerService
{
    public List<(int, int)> PickCore(List<(int, int)> eventList)
    {
        var maxPerformanceProcessorList = eventList
    .Where(pair => pair.Item1 % 2 == 0)  // Filter only even processors
    .OrderByDescending(pair => pair.Item2)  // Then order by the second item (performance)
    .ToList();

        Console.WriteLine("\n" + "Processor order:");
        Console.WriteLine(string.Join(" > ", maxPerformanceProcessorList.Select(pair => $"Processor {pair.Item1}")));

        return maxPerformanceProcessorList;

    }
}