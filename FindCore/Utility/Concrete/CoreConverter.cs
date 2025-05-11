using System.Numerics;
using Base.Utility.Abstract;
using FindCore.Utility.Abstract;

namespace FindCore.Utility.Concrete;

public class CoreConverter : ICoreConverterService
{
    private readonly IProcessorUtilityService _processorUtilityService;
    public CoreConverter(IProcessorUtilityService processorUtilityService)
    {
        _processorUtilityService = processorUtilityService;
    }

    public int[] ConvertToHex(List<(int processorNumber, int maxPerformance)> processorList)
    {
        int[] coreList = new int[processorList.Count];

        for (int i = 0; i < processorList.Count; i++)
        {
            byte processorNumber = (byte)processorList[i].processorNumber;
            int coreNumber = 1 << processorNumber;                      // Shift by one bit to get power of 2
            coreList[i] = coreNumber;
        }

        if (_processorUtilityService.GetProcessorInformation().IsSMTEnabled)
        {

            int[] tempCoreList = new int[coreList.Length / 2];          // If SMT is enabled, reduce the core list size by half
            for (int i = 0; i < tempCoreList.Length; i++)
            {
                tempCoreList[i] = coreList[i * 2];
            }
            coreList = tempCoreList;
        }

        return coreList;
    }

    public void ConvertToArray(int[] coreNum, byte[] arrayModel)
    {

        BigInteger valueToHex = coreNum[0];
        byte[] tempBytes = valueToHex.ToByteArray();

        int byteSize = tempBytes.Length;                                // Check if value has double zero
        while (byteSize > 1 && tempBytes[byteSize - 1] == 0)
        {
            byteSize--;
        }

        Array.Copy(tempBytes, 0, arrayModel, 0, byteSize);
    }
}

