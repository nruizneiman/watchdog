using System;
using System.ServiceProcess;
using System.Timers;
using System.Collections.Generic;

namespace WindowsWatchdog.Test.WindowsService
{
    public partial class MemoryHogService : ServiceBase
    {
        private Timer timer;
        private List<byte[]> memoryHogList;

        public MemoryHogService()
        {
            InitializeComponent();

            memoryHogList = new List<byte[]>();
        }

        protected override void OnStart(string[] args)
        {
            timer = new Timer();
            timer.Interval = 10000; // 10 seconds
            timer.Elapsed += new ElapsedEventHandler(this.OnTimer);
            timer.Start();
        }

        protected override void OnStop()
        {
            timer.Stop();
            memoryHogList.Clear();
        }

        public void OnTimer(object sender, ElapsedEventArgs args)
        {
            // Allocate 1 GB of memory
            byte[] memoryChunk = new byte[1024 * 1024 * 1024];
            memoryHogList.Add(memoryChunk);
            // Fill the memory with some data
            for (int i = 0; i < memoryChunk.Length; i++)
            {
                memoryChunk[i] = 0xFF;
            }
        }

#if DEBUG
        public void OnDebug()
        {
            OnStart(null);
        }
#endif
    }
}
