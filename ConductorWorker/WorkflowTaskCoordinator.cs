using Microsoft.Extensions.Logging;
using SuperSimpleConductor.ConductorClient;
using SuperSimpleConductor.ConductorWorker.Extensions;
using SuperSimpleConductor.ConductorWorker.WorkflowTasks;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuperSimpleConductor.ConductorWorker
{
   public interface IWorkflowTaskCoordinator
   {
      Task PollConductorQueue(string taskDomain);
      void RegisterWorkflowTask(IWorkflowTask workflowTask);
   }

   public class WorkflowTaskCoordinator : IWorkflowTaskCoordinator
   {
      private readonly List<string> SystemTasks = new List<string>
      {
         "DECISION", "HTTP", "JOIN", "KAFKA_PUBLISH" ,"LAMBDA" ,"SUB_WORKFLOW", "_deciderQueue", "FORK"
      };

      private ConductorApi ConductorApi { get; }
      private IServiceProvider ServiceProvider { get; }
      private ILogger<WorkflowTaskCoordinator> Logger { get; }

      private IDictionary<string, Type> SupportedTaskTypes { get; } = new Dictionary<string, Type>
      {
      };

      public WorkflowTaskCoordinator(ConductorApi conductorApi,
                                     IServiceProvider serviceProvider,
                                     ILogger<WorkflowTaskCoordinator> logger)
      {
         ConductorApi = conductorApi;
         ServiceProvider = serviceProvider;
         Logger = logger;
      }

      public async Task PollConductorQueue(string taskDomain)
      {
         try
         {
            // See if any tasks are ready to be picked up
            Logger.LogInformation("Polling Conductor queue");
            var queue = await ConductorApi.GetQueue();

            foreach (var (taskTypeName, queuedCount) in queue)
            {
               var taskName = taskTypeName;

               // We're only interested in tasks within our domain
               if (taskDomain != null)
               {
                  if (!taskName.StartsWith(taskDomain)) continue;
                  // Get rid of the domain prefix
                  taskName = taskName.Replace($"{taskDomain}:", "");
               }

               // We're not interested in system tasks
               if (SystemTasks.Contains(taskName)) continue;

               // We're only interested in the tasks we registered
               if (!SupportedTaskTypes.TryGetValue(taskName, out Type taskType)) continue;

               if (queuedCount > 0)
               {
                  // Get more information on the task
                  Logger.LogInformation("Polling task {TaskName}", taskName);
                  var conductorTask = await ConductorApi.GetTask(taskName, taskDomain);
                  if (conductorTask == null) continue;

                  var taskId = conductorTask.TaskId;

                  // Tell Conductor we're picking up the task
                  Logger.LogDebug("Sending acknowledgement for task {TaskId} ({TaskName})", taskId, taskName);
                  await ConductorApi.SendAcknowledgement(taskId);

                  try
                  {
                     // Process task 
                     Logger.LogInformation("Processing task {TaskId} ({TaskName})", taskId, taskName);
                     var workflowTask = (IWorkflowTask)ServiceProvider.GetService(taskType);
                     var taskResult = await workflowTask.Execute(conductorTask);

                     // Tell Conductor processing succeeded
                     Logger.LogInformation("Updating status for task {TaskId} ({Status})", taskId, taskResult.Status);
                     await ConductorApi.UpdateTask(taskResult);
                  }
                  catch (Exception ex)
                  {
                     // Tell Conductor processing failed
                     var taskResult = conductorTask.FailedWithTerminalError(ex);
                     Logger.LogInformation("Updating status for task {TaskId} ({Status})", taskId, taskResult.Status);
                     await ConductorApi.UpdateTask(taskResult);
                  }
               }
            }
         }
         catch (Exception ex)
         {
            Logger.LogError(ex, "Error calling Conductor api");
         }
      }

      public void RegisterWorkflowTask(IWorkflowTask workflowTask)
      {
         var taskType = workflowTask.GetType();
         var taskName = taskType.Name;
         SupportedTaskTypes.Add(taskName, taskType);
      }
   }
}
