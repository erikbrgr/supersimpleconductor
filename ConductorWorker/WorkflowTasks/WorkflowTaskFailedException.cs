using System;

namespace SuperSimpleConductor.ConductorWorker.WorkflowTasks
{
   public class WorkflowTaskFailedException : Exception
   {
      public WorkflowTaskFailedException(string message) : base(message)
      {
      }

      public WorkflowTaskFailedException(string message, Exception innerException) : base(message, innerException)
      {
      }
   }
}
