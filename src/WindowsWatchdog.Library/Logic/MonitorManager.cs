using System.Collections.Generic;
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
            foreach (var monitor in _monitors)
            {
                monitor.Monitor();
            }
        }
    }
}
