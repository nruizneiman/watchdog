using System;
using System.ServiceProcess;
using System.Threading.Tasks;
using WindowsService.Library.Interfaces;
using WindowsWatchdog.Library.Config;
using WindowsWatchdog.Library.Handlers;

namespace WindowsService.Library.Monitors
{
    public class MemoryMonitor : IServiceMonitor
    {
        private readonly Configuration _config;
        private readonly WindowsServiceManager _wsm;
        private readonly ProcessManager _processManager;

        public MemoryMonitor(Configuration config)
        {
            _config = config;
            _wsm = new WindowsServiceManager();
            _processManager = new ProcessManager();
        }

        public async Task Monitor()
        {
            var itemsToHandle = _config.Monitors.Memory;
            Parallel.ForEach(itemsToHandle, item =>
            {
                switch (item.ItemType)
                {
                    case ItemType.Service:
                        HandleService(item);
                        break;
                    case ItemType.Process:
                        // Handle process status if needed
                        break;
                    case ItemType.IISSite:
                        // Handle IIS site status if needed
                        break;
                }
            });
        }

        private void HandleService(Memory item)
        {
            var canParseThreshold = long.TryParse(item.MemoryThreshold, out var threshold);

            if (canParseThreshold)
            {
                var service = _wsm.GetService(item.ItemName);

                if (service.Status == ServiceControllerStatus.Running)
                {
                    var process = _processManager.GetProcess(item.ItemName);
                    if (process != null)
                    {
                        var memoryUsage = process.WorkingSet64;

                        if (memoryUsage > threshold)
                        {
                            _wsm.RestartService(item.ItemName);
                        }
                    }
                }
            }
        }
    }
}
