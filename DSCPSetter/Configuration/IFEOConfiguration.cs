namespace DSCPSetter.Configuration;

internal class IFEOConfiguration
{
    /*
    CPU Priority
    1	Idle
    2	Normal
    3	High
    4	Realtime
    5	Below Normal
    6	Above Normal

    IO Priority:
    0	Very Low
    1	Low
    2	Normal
    3	High
    4	Critical

    Page Priority
    0 Idle
    1 Very Low
    2 Low
    3 Background    
    4 Background
    5 Normal
     */
    internal Dictionary<string, int> IFEOValues = new Dictionary<string, int>()
    {
        { "CpuPriorityClass", 3},
        { "IoPriority", 3 },
        { "PagePriority", 5 }
    };
}

