using Newtonsoft.Json;

namespace SuperSimpleConductor.ConductorClient.Models
{
   public partial class ConductorWorkflowTask
   {
      [JsonProperty("name")]
      public string Name { get; set; }
      [JsonProperty("taskReferenceName")]
      public string TaskReferenceName { get; set; }
      [JsonProperty("inputParameters")]
      public ConductorTaskData InputParameters { get; set; }
      [JsonProperty("type")]
      public string Type { get; set; }
      [JsonProperty("startDelay")]
      public long StartDelay { get; set; }
      [JsonProperty("optional")]
      public bool Optional { get; set; }
      [JsonProperty("taskDefinition")]
      public ConductorWorkflowTaskDefinition TaskDefinition { get; set; }
      [JsonProperty("asyncComplete")]
      public bool AsyncComplete { get; set; }
   }
}
