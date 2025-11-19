using System;
using System.Collections.Generic;
using System.Threading;

namespace OomKillDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("OOMKill Demo Application Starting...");
            Console.WriteLine($"Initial memory: {GC.GetTotalMemory(false) / 1024 / 1024} MB");

            // List to hold allocated memory blocks
            var memoryHog = new List<byte[]>();

            // Allocation size: 10 MB per iteration
            const int blockSize = 10 * 1024 * 1024;
            int iteration = 0;

            try
            {
                while (true)
                {
                    iteration++;

                    // Allocate memory block
                    byte[] block = new byte[blockSize];

                    // Fill with data to ensure memory is actually allocated
                    for (int i = 0; i < block.Length; i += 4096)
                    {
                        block[i] = (byte)(iteration % 256);
                    }

                    memoryHog.Add(block);

                    long totalMemoryMB = GC.GetTotalMemory(false) / 1024 / 1024;
                    Console.WriteLine($"Iteration {iteration}: Allocated {iteration * 10} MB total, GC reports {totalMemoryMB} MB");

                    // Small delay to make the process observable
                    Thread.Sleep(500);
                }
            }
            catch (OutOfMemoryException)
            {
                Console.WriteLine("OutOfMemoryException caught! This shouldn't happen in Kubernetes - OOMKiller should terminate the process first.");
            }
        }
    }
}
