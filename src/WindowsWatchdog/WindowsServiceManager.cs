using System;
using System.Diagnostics;
using System.ServiceProcess;

namespace WindowsWatchdog.Services
{
    public class WindowsServiceManager
    {
        // Get all services
        public ServiceController[] GetAllServices()
        {
            return ServiceController.GetServices();
        }

        // Get a specific service by name
        public ServiceController GetService(string serviceName)
        {
            try
            {
                return new ServiceController(serviceName);
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine($"Service '{serviceName}' not found.");
                return null;
            }
        }

        // Create a new service
        public void CreateService(string serviceName, string displayName, string executablePath)
        {
            try
            {
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = "sc.exe",
                    Arguments = $"create {serviceName} binPath= \"{executablePath}\" DisplayName= \"{displayName}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var process = Process.Start(processStartInfo))
                {
                    process.WaitForExit();
                    Console.WriteLine(process.StandardOutput.ReadToEnd());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to create service '{serviceName}'. Error: {ex.Message}");
            }
        }

        // Update a service (e.g., change its executable path)
        public void UpdateService(string serviceName, string newExecutablePath)
        {
            try
            {
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = "sc.exe",
                    Arguments = $"config {serviceName} binPath= \"{newExecutablePath}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var process = Process.Start(processStartInfo))
                {
                    process.WaitForExit();
                    Console.WriteLine(process.StandardOutput.ReadToEnd());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to update service '{serviceName}'. Error: {ex.Message}");
            }
        }

        // Delete a service
        public void DeleteService(string serviceName)
        {
            try
            {
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = "sc.exe",
                    Arguments = $"delete {serviceName}",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var process = Process.Start(processStartInfo))
                {
                    process.WaitForExit();
                    Console.WriteLine(process.StandardOutput.ReadToEnd());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to delete service '{serviceName}'. Error: {ex.Message}");
            }
        }

        // Start a service
        public void StartService(string serviceName)
        {
            try
            {
                ServiceController service = new ServiceController(serviceName);
                if (service.Status != ServiceControllerStatus.Running)
                {
                    service.Start();
                    service.WaitForStatus(ServiceControllerStatus.Running);
                    Console.WriteLine($"Service '{serviceName}' started successfully.");
                }
                else
                {
                    Console.WriteLine($"Service '{serviceName}' is already running.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to start service '{serviceName}'. Error: {ex.Message}");
            }
        }

        // Stop a service
        public void StopService(string serviceName)
        {
            try
            {
                ServiceController service = new ServiceController(serviceName);
                if (service.Status != ServiceControllerStatus.Stopped)
                {
                    service.Stop();
                    service.WaitForStatus(ServiceControllerStatus.Stopped);
                    Console.WriteLine($"Service '{serviceName}' stopped successfully.");
                }
                else
                {
                    Console.WriteLine($"Service '{serviceName}' is already stopped.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to stop service '{serviceName}'. Error: {ex.Message}");
            }
        }

        // Restart a service
        public void RestartService(string serviceName)
        {
            try
            {
                ServiceController service = new ServiceController(serviceName);
                if (service.Status == ServiceControllerStatus.Running)
                {
                    service.Stop();
                    service.WaitForStatus(ServiceControllerStatus.Stopped);
                }
                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running);
                Console.WriteLine($"Service '{serviceName}' restarted successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to restart service '{serviceName}'. Error: {ex.Message}");
            }
        }

        // Start multiple services
        public void StartServices(string[] serviceNames)
        {
            foreach (string serviceName in serviceNames)
            {
                StartService(serviceName);
            }
        }

        // Stop multiple services
        public void StopServices(string[] serviceNames)
        {
            foreach (string serviceName in serviceNames)
            {
                StopService(serviceName);
            }
        }

        // Restart multiple services
        public void RestartServices(string[] serviceNames)
        {
            foreach (string serviceName in serviceNames)
            {
                RestartService(serviceName);
            }
        }

        // Get the memory usage of a service
        public long GetServiceMemoryUsage(string serviceName)
        {
            try
            {
                ServiceController service = new ServiceController(serviceName);
                if (service.Status != ServiceControllerStatus.Stopped)
                {
                    int processId = GetServiceProcessId(serviceName);
                    if (processId != -1)
                    {
                        using (Process process = Process.GetProcessById(processId))
                        {
                            return process.WorkingSet64; // Memory usage in bytes
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"Service '{serviceName}' is not running.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to get memory usage for service '{serviceName}'. Error: {ex.Message}");
            }
            return -1;
        }

        // Helper method to get the process ID of a service
        private int GetServiceProcessId(string serviceName)
        {
            try
            {
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = "sc.exe",
                    Arguments = $"queryex {serviceName}",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var process = Process.Start(processStartInfo))
                {
                    process.WaitForExit();
                    string output = process.StandardOutput.ReadToEnd();
                    string pidLine = output.Split('\n').FirstOrDefault(line => line.Trim().StartsWith("PID"));
                    if (!string.IsNullOrEmpty(pidLine))
                    {
                        return int.Parse(pidLine.Split(':')[1].Trim());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to get process ID for service '{serviceName}'. Error: {ex.Message}");
            }
            return -1;
        }
    }
}
