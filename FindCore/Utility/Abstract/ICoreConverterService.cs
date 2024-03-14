namespace FindCore.Utility.Abstract;

public interface ICoreConverterService
{
    int[] ConvertToHex(List<(int processorNumber, int maxPerformance)> maxPerformanceProcessorList);
    //void ConvertToArray(int[] coreNum, string arraySource, NICByteArrayModel? nicModel = null, PCIByteArrayModel? pciModel = null, USBByteArrayModel? usbModel = null);
    void ConvertToArray(int[] coreNum, byte[] arrayModel);
}
