using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace WindowsWatchdog.Test.WindowsService
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            if (!Environment.UserInteractive)
            {
                // Startup as service.
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                new MemoryHogService()
                };
                ServiceBase.Run(ServicesToRun);
            }
            else
            {
                // Startup as application
                var service = new MemoryHogService();
                service.OnDebug();
                System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
            }
        }
    }
}
