using FindCore.Model;
using FindCore.Utility.Abstract;

namespace FindCore.Utility.Concrete;
public class CoreSelector : ICoreSelectorService
{
    private readonly ICoreConverterService _converterService;

    public CoreSelector(ICoreConverterService coreConverterService)
    {
        _converterService = coreConverterService;
    }

    public void SelectCores(int[] coreList)
    {
        int[] selectCoreNIC = [1];
        int[] selectCorePCI = [1];
        int[] selectCoreUSB = [1];

        // Giving a placeholder byte for byte arrays
        NICByteArrayModel.NICByteArray = new byte[1];
        PCIByteArrayModel.PCIByteArray = new byte[1];
        USBByteArrayModel.USBByteArray = new byte[1];

        // Select best core for GPU if its not CORE 0
        selectCorePCI[0] = coreList[0] != 1 ? coreList[0] : coreList[1];

        // Select second to worst core for USB if its not CORE 0 and not the same core with GPU
        selectCoreUSB[0] = coreList[0] != 1 && selectCorePCI[0] != coreList[0] ? coreList[coreList.Length - 1] : coreList[coreList.Length - 2];

        // Select second and third best core for NIC if its not CORE 0 and not the same core with GPU and USB
        selectCoreNIC[0] = coreList[0] != 1 && selectCorePCI[0] != coreList[0] && selectCoreUSB[0] != coreList[0] ? coreList[coreList.Length - 3] : coreList[coreList.Length - 4] + coreList[coreList.Length - 3];

        foreach (int coreNIC in selectCoreNIC) { Console.WriteLine("Selected processor for NIC:Processor {0}", Math.Log(coreNIC, 2)); }
        foreach (int coreGPU in selectCorePCI) { Console.WriteLine("Selected processor for GPU:Processor {0}", Math.Log(coreGPU, 2)); }
        foreach (int coreUSB in selectCoreUSB) { Console.WriteLine("Selected processor for USB:Processor {0}", Math.Log(coreUSB, 2)); }

        _converterService.ConvertToArray(selectCoreNIC, NICByteArrayModel.NICByteArray);
        _converterService.ConvertToArray(selectCorePCI, PCIByteArrayModel.PCIByteArray);
        _converterService.ConvertToArray(selectCoreUSB, USBByteArrayModel.USBByteArray);
    }
}

