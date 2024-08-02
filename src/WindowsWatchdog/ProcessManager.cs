using System;
using System.Diagnostics;
using System.Linq;

namespace WindowsWatchdog.Services
{
    public class ProcessManager
    {
        // Get all processes
        public Process[] GetAllProcesses()
        {
            return Process.GetProcesses();
        }

        // Get a specific process by name
        public Process GetProcess(string processName)
        {
            try
            {
                var processes = Process.GetProcessesByName(processName);
                if (processes.Length > 0)
                {
                    return processes[0];
                }
                else
                {
                    Console.WriteLine($"Process '{processName}' not found.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving process '{processName}': {ex.Message}");
                return null;
            }
        }

        // Start a process
        public void StartProcess(string processName, string arguments = "")
        {
            try
            {
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = processName,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var process = Process.Start(processStartInfo))
                {
                    Console.WriteLine($"Process '{processName}' started successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to start process '{processName}'. Error: {ex.Message}");
            }
        }

        // Stop a process
        public void StopProcess(string processName)
        {
            try
            {
                var processes = Process.GetProcessesByName(processName);
                foreach (var process in processes)
                {
                    process.Kill();
                    process.WaitForExit();
                    Console.WriteLine($"Process '{processName}' stopped successfully.");
                }

                if (processes.Length == 0)
                {
                    Console.WriteLine($"Process '{processName}' not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to stop process '{processName}'. Error: {ex.Message}");
            }
        }

        // Stop multiple processes
        public void StopProcesses(string[] processNames)
        {
            foreach (string processName in processNames)
            {
                StopProcess(processName);
            }
        }
    }
}
