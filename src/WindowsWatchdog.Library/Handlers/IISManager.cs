using System;
using System.Diagnostics;
using Microsoft.Web.Administration;

namespace WindowsWatchdog.Library.Handlers
{
    public class IISManager
    {
        private readonly ServerManager serverManager;

        public IISManager()
        {
            serverManager = new ServerManager();
        }

        // Get all sites
        public SiteCollection GetAllSites()
        {
            return serverManager.Sites;
        }

        // Get a specific site by name
        public Site GetSite(string siteName)
        {
            return serverManager.Sites[siteName];
        }

        // Create a new site
        public void CreateSite(string siteName, string bindingInfo, string physicalPath, string appPoolName)
        {
            try
            {
                Site site = serverManager.Sites.Add(siteName, "http", bindingInfo, physicalPath);
                site.ApplicationDefaults.ApplicationPoolName = appPoolName;
                serverManager.CommitChanges();
                Console.WriteLine($"Site '{siteName}' created successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to create site '{siteName}'. Error: {ex.Message}");
            }
        }

        // Update a site (e.g., change physical path)
        public void UpdateSite(string siteName, string newPhysicalPath)
        {
            try
            {
                Site site = serverManager.Sites[siteName];
                if (site != null)
                {
                    site.Applications["/"].VirtualDirectories["/"].PhysicalPath = newPhysicalPath;
                    serverManager.CommitChanges();
                    Console.WriteLine($"Site '{siteName}' updated successfully.");
                }
                else
                {
                    Console.WriteLine($"Site '{siteName}' not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to update site '{siteName}'. Error: {ex.Message}");
            }
        }

        // Delete a site
        public void DeleteSite(string siteName)
        {
            try
            {
                Site site = serverManager.Sites[siteName];
                if (site != null)
                {
                    serverManager.Sites.Remove(site);
                    serverManager.CommitChanges();
                    Console.WriteLine($"Site '{siteName}' deleted successfully.");
                }
                else
                {
                    Console.WriteLine($"Site '{siteName}' not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to delete site '{siteName}'. Error: {ex.Message}");
            }
        }

        // Get all application pools
        public ApplicationPoolCollection GetAllAppPools()
        {
            return serverManager.ApplicationPools;
        }

        // Get a specific application pool by name
        public ApplicationPool GetAppPool(string appPoolName)
        {
            return serverManager.ApplicationPools[appPoolName];
        }

        // Create a new application pool
        public void CreateAppPool(string appPoolName)
        {
            try
            {
                ApplicationPool appPool = serverManager.ApplicationPools.Add(appPoolName);
                serverManager.CommitChanges();
                Console.WriteLine($"Application pool '{appPoolName}' created successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to create application pool '{appPoolName}'. Error: {ex.Message}");
            }
        }

        // Delete an application pool
        public void DeleteAppPool(string appPoolName)
        {
            try
            {
                ApplicationPool appPool = serverManager.ApplicationPools[appPoolName];
                if (appPool != null)
                {
                    serverManager.ApplicationPools.Remove(appPool);
                    serverManager.CommitChanges();
                    Console.WriteLine($"Application pool '{appPoolName}' deleted successfully.");
                }
                else
                {
                    Console.WriteLine($"Application pool '{appPoolName}' not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to delete application pool '{appPoolName}'. Error: {ex.Message}");
            }
        }

        // Start a site
        public void StartSite(string siteName)
        {
            try
            {
                Site site = serverManager.Sites[siteName];
                if (site != null && site.State != ObjectState.Started)
                {
                    site.Start();
                    serverManager.CommitChanges();
                    Console.WriteLine($"Site '{siteName}' started successfully.");
                }
                else
                {
                    Console.WriteLine($"Site '{siteName}' is already started or not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to start site '{siteName}'. Error: {ex.Message}");
            }
        }

        // Stop a site
        public void StopSite(string siteName)
        {
            try
            {
                Site site = serverManager.Sites[siteName];
                if (site != null && site.State != ObjectState.Stopped)
                {
                    site.Stop();
                    serverManager.CommitChanges();
                    Console.WriteLine($"Site '{siteName}' stopped successfully.");
                }
                else
                {
                    Console.WriteLine($"Site '{siteName}' is already stopped or not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to stop site '{siteName}'. Error: {ex.Message}");
            }
        }

        // Start an application pool
        public void StartAppPool(string appPoolName)
        {
            try
            {
                ApplicationPool appPool = serverManager.ApplicationPools[appPoolName];
                if (appPool != null && appPool.State != ObjectState.Started)
                {
                    appPool.Start();
                    serverManager.CommitChanges();
                    Console.WriteLine($"Application pool '{appPoolName}' started successfully.");
                }
                else
                {
                    Console.WriteLine($"Application pool '{appPoolName}' is already started or not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to start application pool '{appPoolName}'. Error: {ex.Message}");
            }
        }

        // Stop an application pool
        public void StopAppPool(string appPoolName)
        {
            try
            {
                ApplicationPool appPool = serverManager.ApplicationPools[appPoolName];
                if (appPool != null && appPool.State != ObjectState.Stopped)
                {
                    appPool.Stop();
                    serverManager.CommitChanges();
                    Console.WriteLine($"Application pool '{appPoolName}' stopped successfully.");
                }
                else
                {
                    Console.WriteLine($"Application pool '{appPoolName}' is already stopped or not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to stop application pool '{appPoolName}'. Error: {ex.Message}");
            }
        }

        // Get the memory usage of an application pool
        public long GetAppPoolMemoryUsage(string appPoolName)
        {
            try
            {
                ApplicationPool appPool = serverManager.ApplicationPools[appPoolName];
                if (appPool != null)
                {
                    var workerProcesses = appPool.WorkerProcesses;
                    long totalMemoryUsage = 0;

                    foreach (var workerProcess in workerProcesses)
                    {
                        using (var process = Process.GetProcessById(workerProcess.ProcessId))
                        {
                            totalMemoryUsage += process.WorkingSet64;
                        }
                    }

                    return totalMemoryUsage;
                }
                else
                {
                    Console.WriteLine($"Application pool '{appPoolName}' not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to get memory usage for application pool '{appPoolName}'. Error: {ex.Message}");
            }
            return -1;
        }
    }
}
