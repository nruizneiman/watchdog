using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json;

namespace WindowsWatchdog.Config
{
    public class Configuration
    {
        [JsonProperty("agentname")]
        public string AgentName { get; set; }

        [JsonProperty("monitors")]
        public Monitors Monitors { get; set; }
    }

    public class Monitors
    {
        [JsonProperty("memory")]
        public List<Memory> Memory { get; set; }

        [JsonProperty("status")]
        public List<Status> Status { get; set; }
    }

    public class Memory
    {
        [JsonProperty("item-type")]
        public string ItemType { get; set; }

        [JsonProperty("item-name")]
        public string ItemName { get; set; }

        [JsonProperty("memory-threshold")]
        public string MemoryThreshold { get; set; }
    }

    public class Status
    {
        [JsonProperty("item-type")]
        public string ItemType { get; set; }

        [JsonProperty("item-name")]
        public string ItemName { get; set; }

        [JsonProperty("keep")]
        public string Keep { get; set; }
    }
}
