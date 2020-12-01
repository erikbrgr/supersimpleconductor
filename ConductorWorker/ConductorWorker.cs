using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SuperSimpleConductor.ConductorWorker.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SuperSimpleConductor.ConductorWorker
{
   public class ConductorWorker : BackgroundService
   {
      private ConductorSettings Settings { get; }
      private IWorkflowTaskCoordinator WorkflowTaskCoordinator { get; }

      public ConductorWorker(IOptions<ConductorSettings> conductorSettings,
                             IWorkflowTaskCoordinator workflowTaskCoordinator)
      {
         Settings = conductorSettings?.Value ?? throw new ArgumentNullException(nameof(conductorSettings));
         WorkflowTaskCoordinator = workflowTaskCoordinator ?? throw new ArgumentNullException(nameof(workflowTaskCoordinator)); ;
      }

      protected override async Task ExecuteAsync(CancellationToken stoppingToken)
      {
         while (!stoppingToken.IsCancellationRequested)
         {
            await WorkflowTaskCoordinator.PollConductorQueue(Settings.TaskDomain);

            await Task.Delay(Settings.QueuePollingIntervalInSeconds * 1000, stoppingToken);
         }
      }
   }
}
