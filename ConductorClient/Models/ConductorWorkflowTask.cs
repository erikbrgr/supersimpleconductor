namespace SuperSimpleConductor.ConductorClient.Models
{
   public partial class ConductorWorkflowTask
   {
      public string Name { get; set; }
      public string TaskReferenceName { get; set; }
      public ConductorTaskData InputParameters { get; set; }
      public string Type { get; set; }
      public long StartDelay { get; set; }
      public bool Optional { get; set; }
      public ConductorWorkflowTaskDefinition TaskDefinition { get; set; }
      public bool AsyncComplete { get; set; }
   }
}
