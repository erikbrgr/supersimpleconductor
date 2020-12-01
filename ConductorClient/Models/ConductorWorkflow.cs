using System;

namespace SuperSimpleConductor.ConductorClient.Models
{
   public partial class ConductorWorkflow
   {
      public string WorkflowName { get; set; }
      public Guid WorkflowId { get; set; }
      public Guid? ParentWorkflowId { get; set; }
   }
}
