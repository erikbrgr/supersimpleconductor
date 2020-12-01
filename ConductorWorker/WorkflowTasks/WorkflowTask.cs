using Microsoft.Extensions.Logging;
using Serilog.Context;
using SuperSimpleConductor.ConductorClient.Models;
using SuperSimpleConductor.ConductorWorker.Extensions;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SuperSimpleConductor.ConductorWorker.WorkflowTasks
{
   public abstract class WorkflowTask<T> : IWorkflowTask
   {
      protected Guid WorkflowInstanceId { get; private set; }
      protected ConductorTaskData InputData { get; private set; } = new ConductorTaskData();
      public virtual string TaskType => GetType().Name;
      public virtual int? Priority => null;

      protected ILogger<T> Logger { get; }

      public WorkflowTask(ILogger<T> logger)
      {
         Logger = logger;
      }

      protected abstract Task<ConductorTaskData> OnExecute();

      public async Task<ConductorTaskResult> Execute(ConductorTask task)
      {
         if (task is null)
         {
            throw new ArgumentNullException(nameof(task));
         }

         WorkflowInstanceId = task.WorkflowInstanceId;

         var conductorContext = new
         {
            WorkflowInstanceId,
            task.TaskId,
            TaskName = task.WorkflowTask.Name,
            TaskType,
            task.InputData
         };

         using (LogContext.PushProperty("ConductorContext", conductorContext, true))
         {
            try
            {
               Logger.LogInformation("Starting task {TaskType} ({TaskId})", TaskType, task.TaskId);

               if (task.InputData != null)
               {
                  InputData = new ConductorTaskData(task.InputData);
               }

               var stopwatch = Stopwatch.StartNew();

               var output = await OnExecute();

               stopwatch.Stop();

               Logger.LogInformation("Finished task {TaskType} {TaskId} in {Milliseconds}ms", TaskType, task.TaskId, stopwatch.ElapsedMilliseconds);

               if (output == null)
               {
                  output = new ConductorTaskData();
               }
               output["correlationId"] = WorkflowInstanceId;

               return task.Completed(output);
            }
            catch (Exception ex)
            {
               var input = task.InputData != null ? string.Join("; ", task.InputData.Select(i => $"{i.Key}: {i.Value}")) : "";

               Logger.LogError(ex, "Failed to execute {TaskType}. Input {Input}", TaskType, input);

               return ex.GetType() == typeof(WorkflowTaskFailedException)
                   ? task.Failed(ex.Message, null)
                   : task.FailedWithTerminalError(ex, null);
            }
         }
      }
   }
}