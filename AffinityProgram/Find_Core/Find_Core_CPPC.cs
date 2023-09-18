using AffinityProgram.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AffinityProgram.Find_Core
{

    internal class Find_Core_CPPC
    {
        public static byte[] GPUhexBytes;
        public static byte[] USBhexBytes;
        static int processorCount;
        static bool IsSmtEnabled = View.MainMenu.isSmtEnabled;

        public static int[] selectedCoreNIC;
        public void FindCoreCPPC()
        {
            Console.WriteLine("This program uses event viewer to find out preffered core tags. You need to enable CPPC and CPPC preffered cores in bios");

            Console.WriteLine("Press any key to continue...");
            Console.ReadLine();


            using (EventLog eventLog = new EventLog("System"))
            {
                List<string> outputList = new List<string>();
                List<(int maxPerformance, string processorNumber)> maxPerformanceProcessorList = new List<(int, string)>();

                foreach (EventLogEntry entry in eventLog.Entries)
                {
                    if (entry.InstanceId == 55)
                    {
                        string message = entry.Message;

                        string processorNumber = message.Substring(26, 2);
                        string maxPerformance = message.Substring(message.IndexOf("Maximum performance percentage: ") + 32, 3);
                        int maxPerformanceValue = int.Parse(maxPerformance);

                        string output = $"Processor {processorNumber}\nMaximum performance percentage: {maxPerformance}";
                        outputList.Add(output);
                        maxPerformanceProcessorList.Add((maxPerformanceValue, processorNumber));

                        //Loop core count
                        if (IsSmtEnabled)
                            processorCount = View.MainMenu.logicalCoreCount;
                        else
                            processorCount = View.MainMenu.physicalCoreCount;

                        if (outputList.Count >= processorCount)
                            break;
                    }
                }


                maxPerformanceProcessorList.Sort((x, y) => y.maxPerformance.CompareTo(x.maxPerformance));

                foreach (string output in outputList)
                {
                    Console.WriteLine(output);
                }

                if (maxPerformanceProcessorList.Count < 1)
                    Console.WriteLine("\n" + "Error reading Event Viewer. Make sure you didn't disabled needed services and do not use any kind of Log Clearer script.");
                else
                {
                    Console.WriteLine("\n" + "Windows determined processor order:");
                    Console.WriteLine(string.Join(" > ", maxPerformanceProcessorList.Select(pair => $"Processor {pair.processorNumber}")));
                }

                //Convert to hex
                //  |   |   |   |   |   |


                //Take the processor number in order to a list
                List<byte[]> preferredCoreList = new List<byte[]>();
                foreach (var item in maxPerformanceProcessorList.Select(pair => pair.processorNumber))
                {
                    byte processorNumber = byte.Parse(item);
                    preferredCoreList.Add(new byte[] { processorNumber });
                }

                ////Shift bit by one to left to get power of 2
                int coreNumber = 0;
                //BigInteger[] coreList = new BigInteger[preferredCoreList.Count];
                int[] tempCoreList = new int[maxPerformanceProcessorList.Count];
                int index = 0;

                foreach (var item in preferredCoreList)
                {
                    foreach (var squareCore in item)
                    {
                        coreNumber = 1 << squareCore;
                        tempCoreList[index] = coreNumber;
                        index++;
                    }
                }

                int[] coreList;
                if (IsSmtEnabled)
                {
                    coreList = new int[tempCoreList.Length / 2];
                    for (int i = 0; i < tempCoreList.Length; i += 2)
                    {
                        coreList[i / 2] = tempCoreList[i];
                    }
                }
                else
                {
                    coreList = new int[tempCoreList.Length];
                    for (int i = 0; i < tempCoreList.Length; i++)
                    {
                        coreList[i] = tempCoreList[i];
                    }
                }


                //  |   |   |   |   |   |


                List<int> selectCoreGPU = new List<int>();
                List<int> selectCoreUSB = new List<int>();
                List<int> selectCoreNIC = new List<int>();

                //Check if smt is true
                switch (IsSmtEnabled)
                {
                    case true:

                        for (int i = 0; i < coreList.Length; i++)
                        {
                            if (selectCoreGPU.Count == 0)
                            {
                                selectCoreGPU.Add(coreList[0] != 1 ? coreList[0] : coreList[1]);
                            }

                            if (selectCoreUSB.Count == 0)
                            {
                                //Selecting second to last index for USB. 
                                selectCoreUSB.Add(coreList[0] != 1 && selectCoreGPU[0] != coreList[0] ? coreList[coreList.Length - 1] : coreList[coreList.Length - 2]);
                            }
                        }
                        break;
                    case false:

                        for (int i = 0; i < coreList.Length; i++)
                        {
                            if (selectCoreGPU.Count == 0)
                            {
                                selectCoreGPU.Add(coreList[0] != 1 ? coreList[0] : coreList[1]);
                            }

                            if (selectCoreUSB.Count == 0)
                            {
                                selectCoreUSB.Add(coreList[0] != 1 && selectCoreGPU[0] != coreList[0] ? coreList[coreList.Length - 1] : coreList[coreList.Length - 2]);
                            }

                            if (selectCoreNIC.Count == 0)
                            {
                                selectCoreNIC.Add(coreList[0] != 1 && selectCoreGPU[0] != coreList[0] && selectCoreUSB[0] != coreList[0] ? coreList[coreList.Length - 4] : coreList[coreList.Length - 3]);
                            }

                        }
                        break;
                    default:
                        Console.WriteLine("Error selecting cores for USB and GPU");
                        break;
                }

                //Converting to array.
                int[] selectedCoreGPU = selectCoreGPU.ToArray();
                int[] selectedCoreUSB = selectCoreUSB.ToArray();
                selectedCoreNIC = selectCoreNIC.ToArray();
                
                //  |   |   |   |   |   |

                convertArray(selectedCoreGPU, "GPU");
                Model_PciDevices model_PciDevices = new Model_PciDevices(GPUArray: GPUhexBytes);


                convertArray(selectedCoreUSB, "USB");
                Model_UsbDevices model_UsbDevices = new Model_UsbDevices(USBArray: USBhexBytes);

                Model_NicDevices model_NicDevices = new Model_NicDevices(NicArray: selectedCoreNIC);

                foreach (var coreGPU in selectCoreGPU) { Console.WriteLine("Selected processor for GPU:Processor {0}", Math.Log(coreGPU, 2)); }
                foreach (var coreUSB in selectCoreUSB) { Console.WriteLine("Selected processor for USB:Processor {0}", Math.Log(coreUSB, 2)); }
                foreach (var coreUSB in selectCoreUSB) { Console.WriteLine("Selected processor for NIC:Processor {0}", Math.Log(selectedCoreNIC[0], 2)); }

                // Prompt user to exit the program
                Console.WriteLine("\n" + "Press any key to return menu...");
                Console.ReadLine();
            }

        }
        public static void convertArray(int[] coreNum, string ArraySource)
        {
            BigInteger value = coreNum[0];
            byte[] tempBytes = value.ToByteArray();

            // Check if value has double zero
            int byteSize = tempBytes.Length;
            while (byteSize > 1 && tempBytes[byteSize - 1] == 0)
            {
                byteSize--;
            }

            // Add new values to a new array
            if (ArraySource == "GPU")
            {
                GPUhexBytes = new byte[byteSize];
                Array.Copy(tempBytes, 0, GPUhexBytes, 0, byteSize);                
            }
            else
            {
                USBhexBytes = new byte[byteSize];
                Array.Copy(tempBytes, 0, USBhexBytes, 0, byteSize);
            }
        }
    }
}
