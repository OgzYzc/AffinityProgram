using AffinityProgram.Controller.Controller_List;
using AffinityProgram.Controller.Controller_Set;
using AffinityProgram.Controller.Controller_SetInterruptPriority;
using AffinityProgram.Controller.Controller_SetMsiLimit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AffinityProgram.View
{
    internal class MainMenu
    {
        public static void Run()
        {
            //Disable scroll bar
            //Console.WindowHeight = 30;
            //Console.BufferHeight = Console.WindowHeight;

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
                "Benchmark",
            };

            //There may be a better way to do this but I CANNOT care anymore
            //Submenu options for NIC
            string[] subOptions = new string[]
            {
                "Add affinity",
                "Show devices",
                "Set interrupt priority",
                "Set message limit",
            };
            //Submenu options for PCI
            string[] subOptions2 = new string[]
            {
                "Add affinity",
                "Show devices",
                "Set interrupt priority",
                "Set message limit",
            };
            //Submenu options for USB
            string[] subOptions3 = new string[] {
                "Add affinity",
                "Show devices",
                "Set interrupt priority",
                "Set message limit",
            };
            string[] subOptions4 = new string[]
            {
                "Find fastest core",
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
                                    default:
                                        break;
                                }
                                break;
                            case 3:
                                switch (selectedIndex)
                                {
                                    case 0:
                                        Benchmark.Benchmark_FastestCore benchmark_FastestCore = new Benchmark.Benchmark_FastestCore();
                                        benchmark_FastestCore.Run();
                                        ;
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
    }
}