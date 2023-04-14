using AffinityProgram.setDeviceAffinity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AffinityProgram
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string Exit = "";
            while (Exit != "Y")
            {
                try
                {
                    Console.WriteLine("Press 1 for USB affinity, Press 2 for GPU affinity, Press 3 For NIC affinity");
                    byte choice = byte.Parse(Console.ReadLine());
                    switch (GetUserInput(choice))
                    {
                        case "USB":
                            setUSBDevices.setUSBDevicesAffinity();
                            break;
                        case "PCI":
                            setPCIDevices.setPCIDevicesAffinity();
                            break;
                        case "NIC":
                            setNICDevices.setNICDevicesAffinity();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Console.WriteLine("");
                Console.WriteLine("Want to exit? (Y/N):");
                Exit = Console.ReadLine().ToUpper();
            }

            string GetUserInput(byte userChoice) => userChoice switch
            {
                byte i when i == 1 => "USB",
                byte i when i > 1 && i <= 2 => "PCI",
                byte i when i > 2 && i <= 5 => "NIC",
                _ => "Wrong choice"
            };
        }
    }
}
