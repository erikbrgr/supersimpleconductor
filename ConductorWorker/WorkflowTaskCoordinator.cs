using Microsoft.Extensions.Logging;
using Refit;
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
      Task PollConductorQueue(string taskDomain, bool sendAcknowledgement);
      void RegisterWorkflowTask(IWorkflowTask workflowTask);
   }

   public class WorkflowTaskCoordinator : IWorkflowTaskCoordinator
   {
      private readonly List<string> SystemTasks = new List<string>
      {
         "DECISION", "HTTP", "JOIN", "KAFKA_PUBLISH" ,"LAMBDA" ,"SUB_WORKFLOW", "_deciderQueue", "FORK"
      };

      private ConductorApi ConductorApi { get; set; }
      private IServiceProvider ServiceProvider { get; }
      public IConductorApiExceptionHandler ConductorApiExceptionHandler { get; }
      private ILogger<WorkflowTaskCoordinator> Logger { get; }

      private IDictionary<string, Type> SupportedTaskTypes { get; } = new Dictionary<string, Type>
      {
      };

      public WorkflowTaskCoordinator(ConductorApi conductorApi,
                                     IServiceProvider serviceProvider,
                                     ILogger<WorkflowTaskCoordinator> logger,
                                     IConductorApiExceptionHandler conductorApiExceptionHandler = null)
      {
         ConductorApi = conductorApi;
         ServiceProvider = serviceProvider;
         ConductorApiExceptionHandler = conductorApiExceptionHandler;
         Logger = logger;
      }

      public async Task PollConductorQueue(string taskDomain, bool sendAcknowledgement)
      {
         try
         {
            // See if any tasks are ready to be picked up
            Logger.LogInformation("Polling Conductor queue (task domain: {TaskDomain})", taskDomain);
            var queue = await ConductorApi.GetQueue();

            foreach (var (taskTypeName, queuedCount) in queue)
            {
               if (queuedCount > 0)
               {
                  var taskName = taskTypeName;

                  // We're not interested in system tasks
                  if (SystemTasks.Contains(taskName))
                  {
                     Logger.LogDebug("Task {TaskName} is a system task. Skipping", taskName);
                     continue;
                  }

                  Logger.LogDebug("Determining eligibility of task {TaskName}", taskName);

                  // We're only interested in tasks within our domain
                  if (taskDomain != null)
                  {
                     if (!taskName.StartsWith(taskDomain)) continue;

                     // Get rid of the domain prefix
                     Logger.LogDebug("Removing task domain {TaskDomain} from task {TaskName}", taskDomain, taskName);

                     taskName = taskName.Replace($"{taskDomain}:", "");
                  }

                  // We're only interested in the tasks we registered
                  if (!SupportedTaskTypes.TryGetValue(taskName, out Type taskType))
                  {
                     Logger.LogInformation("Task {TaskName} is not registered with this worker. Skipping", taskName);
                     continue;
                  }

                  // Get more information on the task
                  Logger.LogInformation("Polling task {TaskName}", taskName);
                  var conductorTask = await ConductorApi.GetTask(taskName, taskDomain);
                  if (conductorTask == null) continue;

                  var taskId = conductorTask.TaskId;

                  if (sendAcknowledgement)
                  {
                     // Tell Conductor we're picking up the task
                     Logger.LogDebug("Sending acknowledgement for task {TaskId} ({TaskName})", taskId, taskName);
                     var isAcknowledgementSuccess = await ConductorApi.SendAcknowledgement(taskId);
                     if (!isAcknowledgementSuccess) continue;
                  }

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
         catch (Exception ex) when (ConductorApiExceptionHandler != null)
         {
            ConductorApi = ConductorApiExceptionHandler.HandleException(ex, ConductorApi) ?? ConductorApi;
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
