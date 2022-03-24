using Newtonsoft.Json;
using System;

namespace SuperSimpleConductor.ConductorClient.Models
{
   public partial class ConductorWorkflow
   {
      [JsonProperty("workflowName")] 
      public string WorkflowName { get; set; }
      [JsonProperty("workflowId")] 
      public Guid WorkflowId { get; set; }
      [JsonProperty("parentWorkflowId")] 
      public Guid? ParentWorkflowId { get; set; }
   }
}
