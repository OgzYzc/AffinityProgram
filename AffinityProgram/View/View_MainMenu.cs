using AffinityProgram.Benchmark;
using AffinityProgram.Controller.Controller_Del;
using AffinityProgram.Controller.Controller_List;
using AffinityProgram.Controller.Controller_Rem;
using AffinityProgram.Controller.Controller_Set;
using AffinityProgram.Controller.Controller_SetInterruptPriority;
using AffinityProgram.Controller.Controller_SetMsiLimit;
using AffinityProgram.Controller.Controller_SetNicRegistry;
using AffinityProgram.Find_Core;
using System;

namespace AffinityProgram.View
{
    internal class MainMenu
    {
        public static bool isSmtEnabled;
        public static void Run()
        {
            //Disable scroll bar
            Console.WindowHeight = 30;
            Console.BufferHeight = Console.WindowHeight;

            //Check physical and logical core count and SMT
            CheckCPU();

            // Calculate the center of the console window
            int centerX = Console.WindowWidth / 2;

            //Disable blinking cursor
            Console.CursorVisible = false;

            // Defining main menu
            string[] mainOptions = new string[]
            {
                "NIC",
                "PCI",
                "USB",
                "Find best core",
            };

            //There may be a better way to do this but I CANNOT care anymore
            //Submenu options for NIC
            string[] subOptions = new string[]
            {
                "Add affinity",
                "Show devices",
                "Set interrupt priority",
                "Set message limit",
                "Powershell settings",
                "Remove affinity",
            };
            //Submenu options for PCI
            string[] subOptions2 = new string[]
            {
                "Add affinity",
                "Show devices",
                "Set interrupt priority",
                "Set message limit",
                "Remove affinity",
                "Remove pci bridge affinity",
            };
            //Submenu options for USB
            string[] subOptions3 = new string[] {
                "Add affinity",
                "Show devices",
                "Set interrupt priority",
                "Set message limit",
                "Remove affinity",
            };
            string[] subOptions4 = new string[]
            {
                "Find preffered core order (CPPC)",
                "Benchmark",
            };

            // Make the first option of main menu selected as default
            int selectedIndex = 0;
            int previousSelectedIndex = -1;

            while (true)
            {

                // Determine which options to display based on whether we're in the main menu or the submenu
                string[] displayOptions = mainOptions;

                switch (previousSelectedIndex)
                {
                    case 0:
                        displayOptions = subOptions;
                        break;
                    case 1:
                        displayOptions = subOptions2;
                        break;
                    case 2:
                        displayOptions = subOptions3;
                        break;
                    case 3:
                        displayOptions = subOptions4;
                        break;
                    default:
                        break;
                }

                // Display the menu options
                for (int i = 0; i < displayOptions.Length; i++)
                {

                    // Set colors
                    ConsoleColor foregroundColor = ConsoleColor.Gray;
                    ConsoleColor backgroundColor = ConsoleColor.Black;

                    if (i == selectedIndex)
                    {
                        foregroundColor = ConsoleColor.Black;
                        backgroundColor = ConsoleColor.Red;
                    }

                    // Calculate the position of the option on the console window
                    int centerY = Console.WindowHeight / 2 - displayOptions.Length / 2 + i;
                    int leftPadding = centerX - displayOptions[i].Length / 2;

                    // Set the console colors and display the option
                    Console.ForegroundColor = foregroundColor;
                    Console.BackgroundColor = backgroundColor;
                    Console.SetCursorPosition(leftPadding, centerY);
                    Console.Write(displayOptions[i]);
                }

                // Wait for a key to be pressed
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                // Handle the key press
                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        Console.ResetColor();
                        selectedIndex = (selectedIndex - 1 + displayOptions.Length) % displayOptions.Length;
                        break;

                    case ConsoleKey.DownArrow:
                        Console.ResetColor();
                        selectedIndex = (selectedIndex + 1) % displayOptions.Length;
                        break;

                    case ConsoleKey.Enter:
                        Console.ResetColor();
                        Console.Clear();
                        switch (previousSelectedIndex)
                        {

                            case 0:
                                //This controls NIC menu suboptions
                                switch (selectedIndex)
                                {
                                    //Add Nic device affinity
                                    case 0:
                                        Controller_SetNicAffinity controller_SetNicDevices = new Controller_SetNicAffinity();
                                        Console.ReadKey(true);
                                        break;
                                    //List Nic devices
                                    case 1:
                                        Controller_ListNicDevices controller_ListNicDevices = new Controller_ListNicDevices();
                                        Console.ReadKey(true);
                                        break;
                                    //Set interrupt priority
                                    case 2:
                                        Controller_SetNicInterruptPriority controller_SetNicInterruptPriority = new Controller_SetNicInterruptPriority();
                                        Console.ReadKey(true);
                                        break;
                                    //Set msi limit
                                    case 3:
                                        Controller_SetNicMsiLimit controller_SetNicMsiLimit = new Controller_SetNicMsiLimit();
                                        Console.ReadKey(true);
                                        break;
                                    //Set necessary powershell and registry attributes
                                    case 4:
                                        Controller_SetNicRegistry controller_SetNicRegistry = new Controller_SetNicRegistry();
                                        Console.ReadKey(true);
                                        break;
                                    case 5:
                                        Controller_RemNicAffinity controller_RemNicAffinity = new Controller_RemNicAffinity();
                                        Console.ReadKey(true);
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case 1:
                                // This controls PCI menu suboptions
                                switch (selectedIndex)
                                {
                                    // Add Pci device affinity
                                    case 0:
                                        Controller_SetPciAffinity controller_SetPciDevices = new Controller_SetPciAffinity();
                                        Console.ReadKey(true);
                                        break;
                                    // List Pci devices
                                    case 1:
                                        Controller_ListPciDevices controller_ListPciDevices = new Controller_ListPciDevices();
                                        Console.ReadKey(true);
                                        break;
                                    //Set interrupt priority
                                    case 2:
                                        Controller_SetPciInterruptPriority controller_SetPciInterruptPriority = new Controller_SetPciInterruptPriority();
                                        Console.ReadKey(true);
                                        break;
                                    //Set msi limit
                                    case 3:
                                        Controller_SetPciMsiLimit controller_SetPciMsiLimit = new Controller_SetPciMsiLimit();
                                        Console.ReadKey(true);
                                        break;
                                    //Remove affinity
                                    case 4:
                                        Controller_RemPciAffinity controller_RemPciAffinity = new Controller_RemPciAffinity();
                                        Console.ReadKey(true);
                                        break;
                                    //Remove Pci bridge affinity
                                    case 5:
                                        Controller_RemPciBridgeAffinity controller_RemPciBridgeAffinity = new Controller_RemPciBridgeAffinity();
                                        Console.ReadKey(true);
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case 2:
                                // This controls USB menu suboptions
                                switch (selectedIndex)
                                {
                                    // Add Usb device affinity
                                    case 0:
                                        Controller_SetUsbAffinity controller_SetUsbDevices = new Controller_SetUsbAffinity();
                                        Console.ReadKey(true);
                                        break;
                                    // List Usb devices
                                    case 1:
                                        Controller_ListUsbDevices controller_ListUsbDevices = new Controller_ListUsbDevices();
                                        Console.ReadKey(true);
                                        break;
                                    //Set interrupt priority
                                    case 2:
                                        Controller_SetUsbInterruptPriority controller_SetUsbInterruptPriority = new Controller_SetUsbInterruptPriority();
                                        Console.ReadKey(true);
                                        break;
                                    //Set msi limit
                                    case 3:
                                        Controller_SetUsbMsiLimit controller_SetUsbMsiLimit = new Controller_SetUsbMsiLimit();
                                        Console.ReadKey(true);
                                        break;
                                    case 4:
                                        Controller_RemUsbAffinity controller_RemUsbAffinity = new Controller_RemUsbAffinity();
                                        Console.ReadKey(true);
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case 3:
                                switch (selectedIndex)
                                {
                                    case 0:

                                        Find_Core_CPPC find_Core_CPPC = new Find_Core_CPPC();
                                        find_Core_CPPC.FindCoreCPPC();
                                        break;
                                    case 1:
                                        Find_Core_Benchmark find_Core_Benchmark = new Find_Core_Benchmark();
                                        find_Core_Benchmark.FindCoreBenchmark();
                                        break;
                                }
                                break;
                            default:
                                // This controls the main menu
                                switch (selectedIndex)
                                {
                                    // Show submenu if select Nic
                                    case 0:
                                        previousSelectedIndex = selectedIndex;
                                        selectedIndex = 0;
                                        break;
                                    // Show submenu if select Pci
                                    case 1:
                                        previousSelectedIndex = selectedIndex;
                                        selectedIndex = 0;
                                        break;
                                    // Show submenu if select Usb
                                    case 2:
                                        previousSelectedIndex = selectedIndex;
                                        selectedIndex = 0;
                                        break;
                                    //Benchmark
                                    case 3:
                                        previousSelectedIndex = selectedIndex;
                                        selectedIndex = 0;
                                        break;
                                    // Handle other options
                                    default:
                                        Console.WriteLine("You selected: " + displayOptions[selectedIndex]);
                                        Console.ReadKey(true);
                                        previousSelectedIndex = selectedIndex;
                                        break;
                                }
                                break;
                        }
                        Console.Clear();
                        break;

                    case ConsoleKey.Backspace:
                        Console.ResetColor();
                        Console.Clear();
                        //Putting this here again because after clearing console screen, want to cpu info only visible on main menu
                        CheckCPU();
                        // Go back to previous menu
                        if (previousSelectedIndex != -1)
                        {
                            selectedIndex = previousSelectedIndex;
                            previousSelectedIndex = -1;
                        }
                        else
                        {
                            return;
                        }
                        break;
                    default:
                        // Ignore other keys
                        break;
                }
            }
        }
        public static int physicalCoreCount = 0;
        public static int logicalCoreCount = 0;
        public static void CheckCPU()
        {
            //Physical core count
            foreach (System.Management.ManagementBaseObject item in new System.Management.ManagementObjectSearcher("Select NumberOfCores from Win32_Processor").Get())
            {
                physicalCoreCount = int.Parse(item["NumberOfCores"].ToString());
            }

            Console.Write($"P:{physicalCoreCount} ", Console.ForegroundColor = ConsoleColor.Green);


            //Thread count
            foreach (System.Management.ManagementBaseObject item in new System.Management.ManagementObjectSearcher("Select NumberOfLogicalProcessors from Win32_Processor").Get())
            {
                logicalCoreCount = int.Parse(item["NumberOfLogicalProcessors"].ToString());
            }

            Console.Write($"L:{logicalCoreCount} |");

            //Check SMT is enabled or not
            isSmtEnabled = !(physicalCoreCount == logicalCoreCount);
            Console.WriteLine(!(isSmtEnabled) ? "SMT is disabled" : "SMT is enabled", Console.ForegroundColor = ConsoleColor.Red);
        }
    }
}