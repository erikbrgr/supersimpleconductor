using System.Collections.Generic;

namespace SuperSimpleConductor.ConductorClient.Models
{
   public partial class ConductorWorkflowTaskDefinition
   {
      public long CreateTime { get; set; }
      public string Name { get; set; }
      public string Description { get; set; }
      public long RetryCount { get; set; }
      public long TimeoutSeconds { get; set; }
      public IEnumerable<string> InputKeys { get; set; }
      public IEnumerable<string> OutputKeys { get; set; }
      public string TimeoutPolicy { get; set; }
      public string RetryLogic { get; set; }
      public long RetryDelaySeconds { get; set; }
      public long ResponseTimeoutSeconds { get; set; }
      public long RateLimitPerFrequency { get; set; }
      public long RateLimitFrequencyInSeconds { get; set; }
   }
}
