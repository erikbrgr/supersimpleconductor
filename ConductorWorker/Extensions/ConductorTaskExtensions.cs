using SuperSimpleConductor.ConductorClient.Models;
using System;
using System.Collections.Generic;

namespace SuperSimpleConductor.ConductorWorker.Extensions
{
   public static class ConductorTaskExtensions
   {
      public static ConductorTaskResult Completed(this ConductorTask task, ConductorTaskData? outputData = null, ICollection<ConductorTaskLog> logs = null)
      {
         return new ConductorTaskResult
         {
            WorkflowInstanceId = task.WorkflowInstanceId,
            TaskId = task.TaskId,
            Status = TaskResultStatus.COMPLETED,
            OutputData = outputData,
            Logs = logs
         };
      }

      public static ConductorTaskResult InProgress(this ConductorTask task, ConductorTaskData? outputData = null, ICollection<ConductorTaskLog> logs = null)
      {
         return new ConductorTaskResult
         {
            WorkflowInstanceId = task.WorkflowInstanceId,
            TaskId = task.TaskId,
            Status = TaskResultStatus.IN_PROGRESS,
            OutputData = outputData,
            Logs = logs
         };
      }

      public static ConductorTaskResult Failed(this ConductorTask task, string errorMessage, ConductorTaskData? outputData = null, ICollection<ConductorTaskLog> logs = null)
      {
         return new ConductorTaskResult
         {
            WorkflowInstanceId = task.WorkflowInstanceId,
            TaskId = task.TaskId,
            Status = TaskResultStatus.FAILED,
            ReasonForIncompletion = errorMessage,
            OutputData = outputData,
            Logs = logs
         };
      }

      public static ConductorTaskResult FailedWithTerminalError(this ConductorTask task, Exception exception, ConductorTaskData? outputData = null, ICollection<ConductorTaskLog> logs = null)
      {
         return new ConductorTaskResult
         {
            WorkflowInstanceId = task.WorkflowInstanceId,
            TaskId = task.TaskId,
            Status = TaskResultStatus.FAILED_WITH_TERMINAL_ERROR,
            ReasonForIncompletion = exception.ToString(),
            OutputData = outputData,
            Logs = logs
         };
      }
   }
}
