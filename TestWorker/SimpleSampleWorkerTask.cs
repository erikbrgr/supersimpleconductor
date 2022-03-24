using Microsoft.Extensions.Logging;
using SuperSimpleConductor.ConductorWorker.WorkflowTasks;
using System.Threading.Tasks;
using SuperSimpleConductor.ConductorClient.Models;

namespace TestWorker
{
   public class SimpleSampleWorkerTask : WorkflowTask<SimpleSampleWorkerTask>
    {
        public SimpleSampleWorkerTask(
            ILogger<SimpleSampleWorkerTask> logger
        ) : base(logger)
        {
        }

        protected override Task<ConductorTaskData> OnExecute()
        {
            Logger.LogInformation(">>> Executing SimpleSampleWorkerTask");
            var result = new ConductorTaskData();
            return Task.FromResult<ConductorTaskData>(result);
        }
    }
}
