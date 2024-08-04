using System.Threading.Tasks;
using WindowsService.Library.Interfaces;
using WindowsWatchdog.Library.Config;

namespace WindowsService.Library.Monitors
{
    public class MemoryMonitor : IServiceMonitor
    {
        private Configuration _config;

        public MemoryMonitor(Configuration config)
        {
            _config = config;
        }

        public async Task Monitor()
        {
            
        }
    }
}
