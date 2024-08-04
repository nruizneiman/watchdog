﻿using Newtonsoft.Json;
using System;
using System.IO;
using System.ServiceProcess;
using WindowsWatchdog.Config;

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
            var config = LoadConfigs(args);

            //var memoryMonitor = new MemoryMonitor();
            //var serviceStatusMonitor = new ServiceStatusMonitor();

            //var monitorManager = new MonitorManager();
            //monitorManager.AddMonitor(memoryMonitor);
            //monitorManager.AddMonitor(serviceStatusMonitor);

            //monitorManager.StartMonitoring();
        }

        protected override void OnStop()
        {

        }

        private Configuration LoadConfigs(string[] args)
        {
            string defaultJsonFilePath = "default-config.json";
            string jsonFilePath;

            if (args?.Length == 1)
            {

                jsonFilePath = args[0];
            }
            else
            {
                Console.WriteLine("No configuration file path provided, using default path.");
                jsonFilePath = defaultJsonFilePath;
            }

            if (!File.Exists(jsonFilePath))
            {
                Console.WriteLine($"The file '{jsonFilePath}' does not exist.");
                return null;
            }

            try
            {
                var jsonContent = File.ReadAllText(jsonFilePath);
                var config = JsonConvert.DeserializeObject<Configuration>(jsonContent);

                // Use the config object as needed
                Console.WriteLine("Configuration Loaded:");
                Console.WriteLine($"Agent Name: {config.AgentName}");
                Console.WriteLine("Memory Monitors:");
                foreach (var monitor in config.Monitors.Memory)
                {
                    Console.WriteLine($"- Item Type: {monitor.ItemType}");
                    Console.WriteLine($"  Item Name: {monitor.ItemName}");
                    Console.WriteLine($"  Memory Threshold: {monitor.MemoryThreshold}");
                }
                Console.WriteLine("Status Monitors:");
                foreach (var monitor in config.Monitors.Status)
                {
                    Console.WriteLine($"- Item Type: {monitor.ItemType}");
                    Console.WriteLine($"  Item Name: {monitor.ItemName}");
                }

                return config;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while reading the JSON file: {ex.Message}");
            }

            return null;
        }

#if DEBUG
        public void OnDebug()
        {
            OnStart(null);
        }
#endif
    }
}
