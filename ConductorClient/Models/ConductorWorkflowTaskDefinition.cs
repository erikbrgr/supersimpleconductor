using Newtonsoft.Json;
using System.Collections.Generic;

namespace SuperSimpleConductor.ConductorClient.Models
{
   public partial class ConductorWorkflowTaskDefinition
   {
      [JsonProperty("createTime")]
      public long CreateTime { get; set; }
      [JsonProperty("name")]
      public string Name { get; set; }
      [JsonProperty("description")]
      public string Description { get; set; }
      [JsonProperty("retryCount")]
      public long RetryCount { get; set; }
      [JsonProperty("timeoutSeconds")]
      public long TimeoutSeconds { get; set; }
      [JsonProperty("inputKeys")]
      public IEnumerable<string> InputKeys { get; set; }
      [JsonProperty("outputKeys")]
      public IEnumerable<string> OutputKeys { get; set; }
      [JsonProperty("timeoutPolicy")]
      public string TimeoutPolicy { get; set; }
      [JsonProperty("retryLogic")]
      public string RetryLogic { get; set; }
      [JsonProperty("retryDelaySeconds")]
      public long RetryDelaySeconds { get; set; }
      [JsonProperty("responseTimeoutSeconds")]
      public long ResponseTimeoutSeconds { get; set; }
      [JsonProperty("rateLimitPerFrequency")]
      public long RateLimitPerFrequency { get; set; }
      [JsonProperty("rateLimitFrequencyInSeconds")]
      public long RateLimitFrequencyInSeconds { get; set; }
   }
}
