using System;
using System.Collections.Generic;
using System.Threading;

namespace WindowsWatchdog.Test.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<byte[]> memoryHogList = new List<byte[]>();
            Timer timer = new Timer(AllocateMemory, memoryHogList, 0, 10000); // Allocate memory every 10 seconds

            Console.WriteLine("Memory Hog Console App is running. Press [Enter] to stop.");
            Console.ReadLine();

            timer.Dispose();
            memoryHogList.Clear();
        }

        private static void AllocateMemory(object state)
        {
            List<byte[]> memoryHogList = (List<byte[]>)state;
            try
            {
                // Allocate 1 GB of memory
                byte[] memoryChunk = new byte[1024 * 1024 * 1024];
                memoryHogList.Add(memoryChunk);

                // Fill the memory with some data
                for (int i = 0; i < memoryChunk.Length; i++)
                {
                    memoryChunk[i] = 0xFF;
                }

                Console.WriteLine("Allocated 1 GB of memory. Total allocations: {0} GB", memoryHogList.Count);
            }
            catch (OutOfMemoryException)
            {
                Console.WriteLine("Out of memory!");
                memoryHogList.Clear();
            }
        }
    }
}
