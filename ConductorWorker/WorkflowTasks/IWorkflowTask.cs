using SuperSimpleConductor.ConductorClient.Models;
using System.Threading.Tasks;

namespace SuperSimpleConductor.ConductorWorker.WorkflowTasks
{
   public interface IWorkflowTask
   {
      Task<ConductorTaskResult> Execute(ConductorTask conductorTask);
   }
}
