﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SuperSimpleConductor.ConductorClient.Models
{
   public static class TaskResultStatus
   {
      public const string COMPLETED = "COMPLETED";
      public const string IN_PROGRESS = "IN_PROGRESS";
      public const string FAILED = "FAILED";
      public const string FAILED_WITH_TERMINAL_ERROR = "FAILED_WITH_TERMINAL_ERROR";
   }

   public class ConductorTaskResult
   {
      [JsonProperty("workflowInstanceId")]
      public Guid WorkflowInstanceId { get; set; }
      [JsonProperty("taskId")]
      public Guid TaskId { get; set; }
      [JsonProperty("workerId", NullValueHandling = NullValueHandling.Ignore)]
      public Guid? WorkerId { get; set; }
      [JsonProperty("status")]
      public string Status { get; set; }
      [JsonProperty("reasonForIncompletion", NullValueHandling = NullValueHandling.Ignore)]
      public string ReasonForIncompletion { get; set; }
      [JsonProperty("outputData", NullValueHandling = NullValueHandling.Ignore)]
      public ConductorTaskData OutputData { get; set; }
      [JsonProperty("logs", NullValueHandling = NullValueHandling.Ignore)]
      public ICollection<ConductorTaskLog> Logs { get; set; }
      [JsonProperty("callbackAfterSeconds")]
      public long CallbackAfterSeconds { get; set; }
   }
}
