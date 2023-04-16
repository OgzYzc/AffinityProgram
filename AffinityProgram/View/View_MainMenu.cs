using AffinityProgram.Controller.Controller_List;
using AffinityProgram.Controller.Controller_Set;
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
            Console.WindowHeight = 30;
            Console.BufferHeight = Console.WindowHeight;

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
                "Benchmark",
            };
            //Submenu options for PCI
            string[] subOptions2 = new string[]
            {
                "Add affinity",
                "Show devices",
                "Suboption 2.3"
            };
            //Submenu options for USB
            string[] subOptions3 = new string[] {
                "Add affinity",
                "Show devices",
                "Suboption 3.3"
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
                // Calculate the center of the console window
                int centerX = Console.WindowWidth / 2;

                // Determine which options to display based on whether we're in the main menu or the submenu
                string[] displayOptions = mainOptions;

                if (previousSelectedIndex == 0)
                {
                    displayOptions = subOptions;
                }
                else if (previousSelectedIndex == 1)
                {
                    displayOptions = subOptions2;
                }
                else if (previousSelectedIndex == 2)
                {
                    displayOptions = subOptions3;
                }
                else if (previousSelectedIndex == 3)
                {
                    displayOptions = subOptions4;
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
                                        Controller_SetNicDevices controller_SetNicDevices = new Controller_SetNicDevices();
                                        Console.ReadKey(true);
                                        break;
                                    //List Nic devices
                                    case 1:
                                        Controller_ListNicDevices controller_ListNicDevices = new Controller_ListNicDevices();
                                        Console.ReadKey(true);
                                        break;
                                    case 2:
                                        Console.WriteLine("You selected suboption 3 for submenu option 1");
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

                                        Controller_SetPciDevices controller_SetPciDevices = new Controller_SetPciDevices();
                                        Console.ReadKey(true);
                                        break;
                                    // List Pci devices
                                    case 1:

                                        Controller_ListPciDevices controller_ListPciDevices = new Controller_ListPciDevices();
                                        Console.ReadKey(true);
                                        break;
                                    case 2:
                                        Console.WriteLine("You selected suboption 3 for submenu option 2");
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

                                        Controller_SetUsbDevices controller_SetUsbDevices = new Controller_SetUsbDevices();
                                        Console.ReadKey(true);
                                        break;
                                    // List Usb devices
                                    case 1:
                                        Controller_ListUsbDevices controller_ListUsbDevices = new Controller_ListUsbDevices();

                                        Console.ReadKey(true);
                                        break;
                                    case 2:
                                        // Suboption 3
                                        Console.WriteLine("You selected suboption 3 for submenu option 3");
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
                                    case 0:
                                        // Show submenu if select Nic
                                        previousSelectedIndex = selectedIndex;
                                        selectedIndex = 0;
                                        break;
                                    case 1:
                                        // Show submenu if select Pci
                                        previousSelectedIndex = selectedIndex;
                                        selectedIndex = 0;
                                        break;
                                    case 2:
                                        // Show submenu if select Usb
                                        previousSelectedIndex = selectedIndex;
                                        selectedIndex = 0;
                                        break;
                                    case 3:
                                        previousSelectedIndex = selectedIndex;
                                        selectedIndex = 0;
                                        break;
                                    default:
                                        // Handle other options
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
