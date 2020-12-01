using System;

namespace SuperSimpleConductor.ConductorClient.Models
{
   public partial class ConductorTask
   {
      public string TaskType { get; set; }
      public string Status { get; set; }
      public ConductorTaskData InputData { get; set; }
      public string ReferenceTaskName { get; set; }
      public long RetryCount { get; set; }
      public long Seq { get; set; }
      public long PollCount { get; set; }
      public string TaskDefName { get; set; }
      public long ScheduledTime { get; set; }
      public long StartTime { get; set; }
      public long EndTime { get; set; }
      public long UpdateTime { get; set; }
      public long StartDelayInSeconds { get; set; }
      public bool Retried { get; set; }
      public bool Executed { get; set; }
      public bool CallbackFromWorker { get; set; }
      public long ResponseTimeoutSeconds { get; set; }
      public Guid WorkflowInstanceId { get; set; }
      public string WorkflowType { get; set; }
      public Guid TaskId { get; set; }
      public long CallbackAfterSeconds { get; set; }
      public ConductorWorkflowTask WorkflowTask { get; set; }
      public long RateLimitPerFrequency { get; set; }
      public long RateLimitFrequencyInSeconds { get; set; }
      public long WorkflowPriority { get; set; }
      public long Iteration { get; set; }
      public long QueueWaitTime { get; set; }
      public bool LoopOverTask { get; set; }
      public string TaskStatus { get; set; }
   }
}
