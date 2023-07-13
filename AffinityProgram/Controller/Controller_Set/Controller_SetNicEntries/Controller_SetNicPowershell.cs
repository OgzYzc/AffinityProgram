using AffinityProgram.Find_Core;
using AffinityProgram.Model;
using System;
using System.Management.Automation;
using System.Threading;

namespace AffinityProgram.Controller.Controller_SetNicPowershell
{
    public class Controller_SetNicPowershell
    {
        public static void Run()
        {
            var selectedCore = Math.Log(Find_Core_CPPC.selectedCoreNIC[0], 2);
            bool IsSmtEnabled = View.MainMenu.isSmtEnabled;

            try
            {
                // Find currently adapter name user using
                using (PowerShell powershell = PowerShell.Create())
                {
                    // Get all physical network adapters' name
                    powershell.AddCommand("Get-NetAdapter").AddParameter("Name", "*").AddParameter("Physical");
                    var adapters = powershell.Invoke();

                    foreach (var adapter in adapters)
                    {
                        // Get the adapter name from the current adapter object
                        string adapterName = adapter.Properties["Name"].Value.ToString();
                        Console.WriteLine($"Adapter name : '{adapterName}' ");

                        try
                        {
                            if (Find_Core_CPPC.selectedCoreNIC == null)
                            {
                                Console.WriteLine("You are adding affinity without using CPPC. " +
                                    "If you enabled CPPC go back to menu and press 'Find best core' then come back." +
                                    "Or you can add predetermined affinity. Press Enter for adding Predetermined affinity.");

                                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                                if (keyInfo.Key == ConsoleKey.Enter)
                                {
                                    if (IsSmtEnabled)
                                    {
                                        // Bind the base processor to CPU for RSS
                                        Console.WriteLine($"Binding the base processor to CPU '4' for RSS on '{adapterName}'.");
                                        powershell.AddCommand("Set-NetAdapterRss")
                                                         .AddParameter("Name", adapterName)
                                                         .AddParameter("BaseProcessorNumber", 4)
                                                         .Invoke();
                                    }
                                    else
                                    {                                        
                                        Console.WriteLine($"Binding the base processor to CPU '2' for RSS on '{adapterName}'.");
                                        powershell.AddCommand("Set-NetAdapterRss")
                                                         .AddParameter("Name", adapterName)
                                                         .AddParameter("BaseProcessorNumber", 2)
                                                         .Invoke();
                                    }
                                }
                                else
                                    break;
                            }
                            else
                            {                                
                                // Bind the base processor to selected CPU for RSS
                                Console.WriteLine($"Binding the base processor to CPU '{selectedCore}' for RSS on '{adapterName}'.");
                                powershell.AddCommand("Set-NetAdapterRss")
                                                 .AddParameter("Name", adapterName)
                                                 .AddParameter("BaseProcessorNumber", selectedCore)
                                                 .Invoke();
                            }

                            // Settings max processors to 2. This set Number of receive queue to 2
                            Console.WriteLine($"Selecting max processors for 2 CPUs '{adapterName}'.");
                            powershell.AddCommand("Set-NetAdapterRss")
                                             .AddParameter("Name", adapterName)
                                             .AddParameter("MaxProcessors", 2)
                                             .Invoke();

                            // Verify the base processor for RSS
                            Console.WriteLine($"Verifying the base processor for RSS on '{adapterName}'.");
                            powershell.Commands.Clear();
                            powershell.AddCommand("Get-NetAdapterRss")
                                             .AddParameter("Name", adapterName);
                            var results = powershell.Invoke();
                            if (results.Count > 0)
                            {
                                var rss = results[0];
                                var baseProcessorNumber = rss.Properties["BaseProcessorNumber"].Value.ToString();
                                if (Find_Core_CPPC.selectedCoreNIC == null)
                                {
                                    if (IsSmtEnabled)
                                    {
                                        if (baseProcessorNumber == "4")
                                        {
                                            Console.WriteLine($"Successfully bound the base processor to CPU '4' for RSS on '{adapterName}'.");
                                        }
                                        else
                                        {
                                            Console.WriteLine($"Failed to bind the base processor to CPU '4' for RSS on '{adapterName}'.");
                                        }
                                    }
                                    else
                                    {
                                        if (baseProcessorNumber == "2")
                                        {
                                            Console.WriteLine($"Successfully bound the base processor to CPU '2' for RSS on '{adapterName}'.");
                                        }
                                        else
                                        {
                                            Console.WriteLine($"Failed to bind the base processor to CPU '2' for RSS on '{adapterName}'.");
                                        }
                                    }                                        
                                }
                                else
                                {
                                    if (baseProcessorNumber == selectedCore.ToString())
                                    {
                                        Console.WriteLine($"Successfully bound the base processor to CPU '{selectedCore}' for RSS on '{adapterName}'.");
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Failed to bind the base processor to CPU '{selectedCore}' for RSS on '{adapterName}'.");
                                    }
                                }
                                
                            }

                            // Restart the current adapter
                            Console.WriteLine($"Restarting adapter '{adapterName}'\nWaiting 10 seconds for the adapter to restart itself.");
                            powershell.Commands.Clear();
                            powershell.AddCommand("Restart-NetAdapter")
                                             .AddParameter("Name", adapterName)
                                             .Invoke();
                            Thread.Sleep(10000);

                            // Check the adapter running or not
                            powershell.Commands.Clear();
                            powershell.AddScript($"Get-NetAdapter | Where-Object {{ $_.Name -eq '{adapterName}' -and $_.Status -eq 'Up' }}");
                            var checkResults = powershell.Invoke();
                            if (checkResults.Count > 0)
                            {
                                Console.WriteLine($"Adapter '{adapterName}' restarted successfully.\nYou can go back now.");
                            }
                            else
                            {
                                Console.WriteLine($"Failed to restart adapter '{adapterName}'.\nGo back and try again.");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"An error occurred while processing adapter '{adapterName}'\nGo back and try again.: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }    
}
