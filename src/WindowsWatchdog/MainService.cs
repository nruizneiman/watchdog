using System.ServiceProcess;
using WindowsService.Library.Logic;
using WindowsService.Library.Monitors;

namespace WindowsWatchdog
{
    public partial class MainService : ServiceBase
    {
        public MainService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            var configLogic = new ConfigLogic();
            var config = configLogic.LoadConfigs(args);

            var memoryMonitor = new MemoryMonitor(config);
            var serviceStatusMonitor = new ServiceStatusMonitor(config);

            var monitorManager = new MonitorManager();
            monitorManager.AddMonitor(memoryMonitor);
            monitorManager.AddMonitor(serviceStatusMonitor);

            monitorManager.StartMonitoring();
        }

        protected override void OnStop()
        {

        }

#if DEBUG
        public void OnDebug()
        {
            OnStart(null);
        }
#endif
    }
}
