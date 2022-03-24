using Newtonsoft.Json;
using System;

namespace SuperSimpleConductor.ConductorClient.Models
{
   public partial class ConductorTask
   {
      [JsonProperty("taskType")]
      public string TaskType { get; set; }
      [JsonProperty("status")]
      public string Status { get; set; }
      [JsonProperty("inputData")]
      public ConductorTaskData InputData { get; set; }
      [JsonProperty("referenceTaskName")]
      public string ReferenceTaskName { get; set; }
      [JsonProperty("retryCount")]
      public long RetryCount { get; set; }
      [JsonProperty("seq")]
      public long Seq { get; set; }
      [JsonProperty("pollCount")]
      public long PollCount { get; set; }
      [JsonProperty("taskDefName")]
      public string TaskDefName { get; set; }
      [JsonProperty("scheduledTime")]
      public long ScheduledTime { get; set; }
      [JsonProperty("startTime")]
      public long StartTime { get; set; }
      [JsonProperty("endTime")]
      public long EndTime { get; set; }
      [JsonProperty("updateTime")]
      public long UpdateTime { get; set; }
      [JsonProperty("startDelayInSeconds")]
      public long StartDelayInSeconds { get; set; }
      [JsonProperty("retried")]
      public bool Retried { get; set; }
      [JsonProperty("executed")]
      public bool Executed { get; set; }
      [JsonProperty("callbackFromWorker")]
      public bool CallbackFromWorker { get; set; }
      [JsonProperty("responseTimeoutSeconds")]
      public long ResponseTimeoutSeconds { get; set; }
      [JsonProperty("workflowInstanceId")]
      public Guid WorkflowInstanceId { get; set; }
      [JsonProperty("workflowType")]
      public string WorkflowType { get; set; }
      [JsonProperty("taskId")]
      public Guid TaskId { get; set; }
      [JsonProperty("callbackAfterSeconds")]
      public long CallbackAfterSeconds { get; set; }
      [JsonProperty("workflowTask")]
      public ConductorWorkflowTask WorkflowTask { get; set; }
      [JsonProperty("rateLimitPerFrequency")]
      public long RateLimitPerFrequency { get; set; }
      [JsonProperty("rateLimitFrequencyInSeconds")]
      public long RateLimitFrequencyInSeconds { get; set; }
      [JsonProperty("workflowPriority")]
      public long WorkflowPriority { get; set; }
      [JsonProperty("iteration")]
      public long Iteration { get; set; }
      [JsonProperty("queueWaitTime")]
      public long QueueWaitTime { get; set; }
      [JsonProperty("loopOverTask")]
      public bool LoopOverTask { get; set; }
      [JsonProperty("taskStatus")]
      public string TaskStatus { get; set; }
   }
}
