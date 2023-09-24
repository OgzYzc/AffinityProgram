using AffinityProgram.Find_Core;
using System;
using System.Linq;
using System.Management.Automation;
using System.Threading;

namespace AffinityProgram.Controller.Controller_SetNicPowershell
{
    public class Controller_SetNicPowershell
    {
        public static void Run()
        {
            bool IsSmtEnabled = View.MainMenu.isSmtEnabled;
            try
            {
                using (PowerShell powershell = PowerShell.Create())
                {
                    PSObject[] adapters = getAllPhysicalAdapters(powershell);
                    foreach (PSObject adapter in adapters)
                    {
                        processAdapter(powershell, adapter, IsSmtEnabled);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static PSObject[] getAllPhysicalAdapters(PowerShell powershell)
        {
            powershell.AddCommand("Get-NetAdapter").AddParameter("Name", "*").AddParameter("Physical");
            return powershell.Invoke().ToArray();
        }

        private static void processAdapter(PowerShell powershell, PSObject adapter, bool IsSmtEnabled)
        {
            string adapterName = adapter.Properties["Name"].Value.ToString();
            Console.WriteLine($"Processing Adapter: '{adapterName}'");

            try
            {
                //NORQ workaround
                setMaxProcessors(powershell, adapterName, 1);

                if (Find_Core_CPPC.selectedCoreNIC == null)
                {
                    predeterminedAffinity(powershell, adapterName, IsSmtEnabled);
                }
                else
                {
                    selectedAffinity(powershell, adapterName);
                }

                restartAdapter(powershell, adapterName);
                setMaxProcessors(powershell, adapterName, 4);
                restartAdapter(powershell, adapterName);

                verifyAdapterStatus(powershell, adapterName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error with adapter '{adapterName}': {ex.Message}");
            }
        }

        private static void setMaxProcessors(PowerShell powershell, string adapterName, int maxProcessors)
        {
            Console.WriteLine($"Selecting max processors for '{maxProcessors}' CPUs '{adapterName}'.");
            powershell.Commands.Clear();
            powershell.AddCommand("Set-NetAdapterRss")
                      .AddParameter("Name", adapterName)
                      .AddParameter("MaxProcessors", maxProcessors)
                      .Invoke();
        }

        private static void predeterminedAffinity(PowerShell powershell, string adapterName, bool IsSmtEnabled)
        {
            Console.WriteLine("You are adding affinity without using CPPC. " +
                "If you enabled CPPC go back to menu and press 'Find best core' then come back." +
                "Or you can add predetermined affinity. Press Enter for adding Predetermined affinity.");

            if (Console.ReadKey(true).Key == ConsoleKey.Enter)
            {
                Console.Clear();
                int baseProcessor = IsSmtEnabled ? 2 : 0;
                int maxProcessor = IsSmtEnabled ? 4 : 4;

                bindBaseProcessor(powershell, adapterName, baseProcessor);
                setMaxProcessors(powershell, adapterName, maxProcessor);
            }
        }

        private static void selectedAffinity(PowerShell powershell, string adapterName)
        {
            int selectedCore = (int)Math.Log(Find_Core_CPPC.selectedCoreNIC[0], 2);
            bindBaseProcessor(powershell, adapterName, selectedCore);
            setMaxProcessors(powershell, adapterName, selectedCore + 1);
        }

        private static void bindBaseProcessor(PowerShell powershell, string adapterName, int baseProcessor)
        {
            Console.WriteLine($"Binding the base processor to CPU '{baseProcessor}' for RSS on '{adapterName}'.");
            powershell.Commands.Clear();
            powershell.AddCommand("Set-NetAdapterRss")
                      .AddParameter("Name", adapterName)
                      .AddParameter("BaseProcessorNumber", baseProcessor)
                      .Invoke();
        }

        private static void restartAdapter(PowerShell powershell, string adapterName)
        {
            Console.WriteLine($"Restarting adapter '{adapterName}'\nWaiting 10 seconds for the adapter to restart.");
            powershell.Commands.Clear();
            powershell.AddCommand("Restart-NetAdapter")
                      .AddParameter("Name", adapterName)
                      .Invoke();
            Thread.Sleep(10000);
        }

        private static void verifyAdapterStatus(PowerShell powershell, string adapterName)
        {
            powershell.Commands.Clear();
            powershell.AddScript($"Get-NetAdapter | Where-Object {{ $_.Name -eq '{adapterName}' -and $_.Status -eq 'Up' }}");
            System.Collections.ObjectModel.Collection<PSObject> checkResults = powershell.Invoke();

            if (checkResults.Count > 0)
            {
                Console.WriteLine($"Adapter '{adapterName}' restarted successfully.\nYou can go back now.");
            }
            else
            {
                Console.WriteLine($"Failed to restart adapter '{adapterName}'.\nGo back and try again.");
            }
        }
    }
}
