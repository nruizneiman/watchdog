using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WindowsService.Library.Interfaces;

namespace WindowsService.Library.Logic
{
    public class MonitorManager
    {
        private readonly List<IServiceMonitor> _monitors;

        public MonitorManager()
        {
            _monitors = new List<IServiceMonitor>();
        }

        public void AddMonitor(IServiceMonitor monitor)
        {
            _monitors.Add(monitor);
        }

        public void StartMonitoring()
        {
            while (true)
            {
                Parallel.ForEach(_monitors, async monitor =>
                {
                    await monitor.Monitor();
                });

                Thread.Sleep(3000);
            }
        }
    }
}
