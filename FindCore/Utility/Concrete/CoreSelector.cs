using FindCore.Model;
using FindCore.Utility.Abstract;

namespace FindCore.Utility.Concrete;
public class CoreSelector : ICoreSelectorService
{
    private readonly ICoreConverterService _converterService;
    public static int FindCoreNIC;
    public CoreSelector(ICoreConverterService coreConverterService)
    {
        _converterService = coreConverterService;
    }

    public void SelectCores(int[] coreList)
    {
        int[] selectCoreNIC = [1];
        int[] selectCorePCI = [1];
        int[] selectCoreUSB = [1];

        int maxCoreValue = coreList.Max();

        // Allocate byte arrays based on the size of the largest core value
        int byteArraySize = (int)Math.Ceiling(Math.Log(maxCoreValue + 1, 2) / 8);

        // Dynamically allocate byte arrays based on calculated size
        NICByteArrayModel.NICByteArray = new byte[byteArraySize];
        PCIByteArrayModel.PCIByteArray = new byte[byteArraySize];
        USBByteArrayModel.USBByteArray = new byte[byteArraySize];

        // Select best core for GPU if its not CORE 0
        selectCorePCI[0] = coreList[0] != 1 ? coreList[0] : coreList[1];

        // Select second to worst core for USB if its not CORE 0 and not the same core with GPU
        selectCoreUSB[0] = coreList[0] != 1 && selectCorePCI[0] != coreList[0] ? coreList[coreList.Length - 1] : coreList[coreList.Length - 2];

        // Select second and third best core for NIC if its not CORE 0 and not the same core with GPU and USB
        selectCoreNIC[0] = coreList[0] != 1 && selectCorePCI[0] != coreList[0] && selectCoreUSB[0] != coreList[0] ? coreList[coreList.Length - 3] : coreList[coreList.Length - 4] + coreList[coreList.Length - 3];

        // Round the selected processor to the nearest lower power of 2, and treat the remaining value as the second core.
        int roundedNIC = (int)Math.Pow(2, Math.Floor(Math.Log(selectCoreNIC[0], 2)));
        int remaining = selectCoreNIC[0] - roundedNIC;
        if (remaining > 0)
        {
            Console.WriteLine("\nSelected processor for NIC: Processor {0} + Processor {1}",
                              (int)Math.Log(roundedNIC, 2), (int)Math.Log(remaining, 2));
            FindCoreNIC = (int)Math.Log(roundedNIC, 2);
        }

        else
            Console.WriteLine("Selected processor for NIC: Processor {0}", (int)Math.Log(roundedNIC, 2));

        //foreach (int coreNIC in selectCoreNIC) { Console.WriteLine("Selected processor for NIC:Processor {0}", Math.Log(coreNIC, 2)); }
        foreach (int coreGPU in selectCorePCI) { Console.WriteLine("Selected processor for GPU: Processor {0}", Math.Log(coreGPU, 2)); }
        
        foreach (int coreUSB in selectCoreUSB) { Console.WriteLine("Selected processor for USB: Processor {0}", Math.Log(coreUSB, 2)); }


        _converterService.ConvertToArray(selectCoreNIC, NICByteArrayModel.NICByteArray);
        _converterService.ConvertToArray(selectCorePCI, PCIByteArrayModel.PCIByteArray);
        _converterService.ConvertToArray(selectCoreUSB, USBByteArrayModel.USBByteArray);
    }

}

