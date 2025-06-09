using System.Diagnostics;
using Base.Utility.Abstract;
using FindCore.Utility.Abstract;

namespace FindCore.Utility.Concrete;

public class EventReader : IEventReaderService
{
    private readonly IProcessorUtilityService _processorUtilityService;
    public EventReader(IProcessorUtilityService processorUtilityService)
    {
        _processorUtilityService = processorUtilityService;
    }

    public List<(int, int)> ReadEventViewer()
    {
        using (EventLog eventLog = new EventLog("System"))
        {
            if (eventLog == null)
            {
                Console.WriteLine("\n" + "Error reading Event Viewer. Make sure you didn't disable needed services and do not use any kind of Log Clearer script.");
                throw new NullReferenceException();
            }
            int coreCount = 0;
            if (_processorUtilityService.GetProcessorInformation().IsSMTEnabled)
                coreCount = _processorUtilityService.GetProcessorInformation().LogicalProcessorCount;
            else
                coreCount = _processorUtilityService.GetProcessorInformation().PhysicalProcessorCount;

            List<(int, int)> messagesSet = new();                       // ProcessNumber, MaxPerformancePercentage

            for (int i = eventLog.Entries.Count - 1; i >= 0; i--)       // Start from latest
            {
                EventLogEntry entry = eventLog.Entries[i];

                if (entry.InstanceId == 55)
                {
                    messagesSet.Add((int.Parse(entry.Message.Substring(26, 2)), int.Parse(entry.Message.Substring(entry.Message.IndexOf("Maximum performance percentage: ") + 32, 3))));

                    if (messagesSet.Count >= coreCount)                 // Loop core count
                        break;
                }
            }

            return messagesSet;
        }
    }
}
