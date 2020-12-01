using Refit;
using SuperSimpleConductor.ConductorClient.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuperSimpleConductor.ConductorClient
{
   public interface ConductorApi
   {
      [Get("/tasks/queue/all")]
      Task<Dictionary<string, int>> GetQueue();

      [Get("/tasks/poll/{taskType}")]
      Task<ConductorTask> GetTask(string taskType, string? domain);

      [Post("/tasks/{taskId}/ack")]
      Task<bool> SendAcknowledgement(Guid taskId, Guid workerId);

      [Post("/tasks/{taskId}/ack")]
      Task<bool> SendAcknowledgement(Guid taskId);

      [Post("/tasks")]
      Task<string> UpdateTask(ConductorTaskResult taskResult);

      [Get("/workflow/{workflowId}")]
      Task<ConductorWorkflow> GetWorkflow(Guid workflowId, bool includeTasks = false);
   }
}
