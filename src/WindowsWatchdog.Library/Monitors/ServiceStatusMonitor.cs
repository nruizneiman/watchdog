using WindowsService.Library.Interfaces;
using WindowsWatchdog.Library.Config;

namespace WindowsService.Library.Monitors
{
    public class ServiceStatusMonitor : IServiceMonitor
    {
        private Configuration _config;

        public ServiceStatusMonitor(Configuration config)
        {
            _config = config;
        }

        public void Monitor()
        {

        }
    }
}
