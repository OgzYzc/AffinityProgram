using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark
{
    public class Benchmark_FastestCore
    {
        public Benchmark_FastestCore()
        {
            
        }
        static void Main(string[] args) { }
        public void Run()
        {
            Console.WriteLine("Welcome to the Core Benchmark program!");

            // Prompt user to start the benchmark
            Console.WriteLine("Press any key to start the benchmark...");
            Console.ReadKey();

            // Check the number of physical cores on the CPU
            int coreCount = Environment.ProcessorCount;
            Console.WriteLine($"This system has {coreCount} physical cores.");

            // Define a complex mathematical operation
            const int numIterations = 1000000;
            Func<double, double, double> mathOperation = (x, y) =>
            {
                double result = 0;
                for (int i = 0; i < numIterations; i++)
                {
                    result += Math.Sqrt(x * Math.PI) * Math.Log10(y);
                }
                return result;
            };
            Console.WriteLine($"The benchmark operation will perform {numIterations} iterations of a complex mathematical operation using Sqrt and Log10 functions.");

            // Benchmark each core 5 times with 5 iterations per run and take the average
            double[] results = new double[coreCount];
            double[] times = new double[coreCount];
            double[] totalTimes = new double[coreCount];
            for (int i = 0; i < coreCount; i++)
            {
                Console.WriteLine($"Benchmarking core {i + 1}...");
                Process.GetCurrentProcess().ProcessorAffinity = new IntPtr(1 << i);
                double totalTime = 0;
                for (int j = 0; j < 5; j++)
                {
                    Console.WriteLine($"Starting run {j + 1}...");
                    double runTime = 0;
                    for (int k = 0; k < 5; k++)
                    {
                        Console.Write($"Core {i + 1}, Iteration {k + 1}: ");
                        var stopwatch = Stopwatch.StartNew();
                        results[i] = mathOperation(2.0, i + 1);
                        stopwatch.Stop();
                        var elapsedMs = stopwatch.Elapsed.TotalMilliseconds;
                        runTime += elapsedMs;
                        Console.WriteLine($"{elapsedMs}ms");
                    }
                    Console.WriteLine($"Core {i + 1}, run {j + 1} took an average of {runTime / 5.0}ms to complete.");
                    totalTime += runTime;
                }
                times[i] = totalTime / 25.0;
                totalTimes[i] = totalTime;
                Console.WriteLine($"Average time for core {i + 1}: {times[i]}ms");
                Console.WriteLine($"Total time for core {i + 1}: {totalTime}ms");
            }

            // Find the fastest core
            double fastestTime = double.MaxValue;
            int fastestIndex = 0;
            for (int i = 0; i < coreCount; i++)
            {
                Console.WriteLine($"Core {i + 1} took an average of {times[i]}ms to complete the operation and produced a result of {results[i]}");
                Console.WriteLine($"Total time for core {i + 1}: {totalTimes[i]}ms");
                if (times[i] < fastestTime)
                {
                    fastestTime = times[i];
                    fastestIndex = i;
                }
            }

            // Display the fastest core to the user
            Console.WriteLine($"Core {fastestIndex + 1} was the fastest, taking an average of {fastestTime}ms to complete the operation and producing a result of {results[fastestIndex]}");

            // Prompt user to exit the program
            Console.WriteLine("Press any key to exit the program...");
            Console.ReadKey();
        }
    }
}
