using System;
using System.Management.Automation;
using System.Threading;

namespace AffinityProgram.Controller.Controller_SetNicPowershell
{
    public class Controller_SetNicPowershell
    {
        public Controller_SetNicPowershell()
        {
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

                        using (PowerShell adapterPowershell = PowerShell.Create())
                        {
                            try
                            {
                                // Bind the base processor to CPU2 (Core 3) for RSS
                                Console.WriteLine($"Binding the base processor to 2 for RSS on '{adapterName}'.");
                                adapterPowershell.AddCommand("Set-NetAdapterRss")
                                                 .AddParameter("Name", adapterName)
                                                 .AddParameter("BaseProcessorNumber", 2)
                                                 .Invoke();

                                // Verify the base processor for RSS
                                Console.WriteLine($"Verifying the base processor for RSS on '{adapterName}'.");
                                adapterPowershell.Commands.Clear();
                                adapterPowershell.AddCommand("Get-NetAdapterRss")
                                                 .AddParameter("Name", adapterName);
                                var results = adapterPowershell.Invoke();
                                if (results.Count > 0)
                                {
                                    var rss = results[0];
                                    var baseProcessorNumber = rss.Properties["BaseProcessorNumber"].Value.ToString();
                                    if (baseProcessorNumber == "2")
                                    {
                                        Console.WriteLine($"Successfully bound the base processor to 2 for RSS on '{adapterName}'.");
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Failed to bind the base processor to 2 for RSS on '{adapterName}'.");
                                    }
                                }

                                // Restart the current adapter
                                Console.WriteLine($"Restarting adapter '{adapterName}'\nWaiting 10 seconds for the adapter to restart itself.");
                                adapterPowershell.Commands.Clear();
                                adapterPowershell.AddCommand("Restart-NetAdapter")
                                                 .AddParameter("Name", adapterName)
                                                 .Invoke();
                                Thread.Sleep(10000);

                                // Check the adapter running or not
                                adapterPowershell.Commands.Clear();
                                adapterPowershell.AddScript($"Get-NetAdapter | Where-Object {{ $_.Name -eq '{adapterName}' -and $_.Status -eq 'Up' }}");
                                var checkResults = adapterPowershell.Invoke();
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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
