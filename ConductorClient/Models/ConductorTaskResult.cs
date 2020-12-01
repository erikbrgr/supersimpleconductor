using System;
using System.Collections.Generic;

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
      public Guid WorkflowInstanceId { get; set; }
      public Guid TaskId { get; set; }
      public Guid? WorkerId { get; set; }
      public string Status { get; set; }
      public string ReasonForIncompletion { get; set; }
      public ConductorTaskData OutputData { get; set; }
      public ICollection<ConductorTaskLog> Logs { get; set; }
      public long CallbackAfterSeconds { get; set; }
   }
}
