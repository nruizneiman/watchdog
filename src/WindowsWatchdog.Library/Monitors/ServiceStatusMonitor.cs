using System;
using System.ServiceProcess;
using System.Threading.Tasks;
using WindowsService.Library.Interfaces;
using WindowsWatchdog.Library.Config;
using WindowsWatchdog.Library.Handlers;

namespace WindowsService.Library.Monitors
{
    public class ServiceStatusMonitor : IServiceMonitor
    {
        private readonly Configuration _config;
        private readonly WindowsServiceManager _wsm;

        public ServiceStatusMonitor(Configuration config)
        {
            _config = config;
            _wsm = new WindowsServiceManager();
        }

        public async Task Monitor()
        {
            var itemsToHandle = _config.Monitors.Status;
            Parallel.ForEach(itemsToHandle, item =>
            {
                switch (item.ItemType)
                {
                    case ItemType.Service:
                        HandleServiceStatus(item);
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

        private void HandleServiceStatus(Status item)
        {
            var service = _wsm.GetService(item.ItemName);

            switch (service.Status)
            {
                case ServiceControllerStatus.Paused:
                    if (item.Keep.Equals(ServiceControllerStatus.Stopped.ToString(), StringComparison.OrdinalIgnoreCase)) _wsm.StopService(item.ItemName);
                    if (item.Keep.Equals(ServiceControllerStatus.Running.ToString(), StringComparison.OrdinalIgnoreCase)) _wsm.StartService(item.ItemName);
                    break;
                case ServiceControllerStatus.Running:
                    if (item.Keep.Equals(ServiceControllerStatus.Stopped.ToString(), StringComparison.OrdinalIgnoreCase)) _wsm.StopService(item.ItemName);
                    break;
                case ServiceControllerStatus.Stopped:
                    if (item.Keep.Equals(ServiceControllerStatus.Running.ToString(), StringComparison.OrdinalIgnoreCase)) _wsm.StartService(item.ItemName);
                    break;
            }
        }
    }
}
