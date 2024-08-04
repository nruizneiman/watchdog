using Newtonsoft.Json;
using System.Collections.Generic;

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

    public abstract class Item
    {
        [JsonProperty("item-type")]
        public string ItemType { get; set; }

        [JsonProperty("item-name")]
        public string ItemName { get; set; }
    }

    public class Memory : Item
    {
        [JsonProperty("memory-threshold")]
        public string MemoryThreshold { get; set; }
    }

    public class Status : Item
    {
        [JsonProperty("keep")]
        public string Keep { get; set; }
    }
}
