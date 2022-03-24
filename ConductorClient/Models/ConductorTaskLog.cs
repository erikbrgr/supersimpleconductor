using Newtonsoft.Json;
using System;

namespace SuperSimpleConductor.ConductorClient.Models
{
   public class ConductorTaskLog
   {
      [JsonProperty("log")] 
      public string Log { get; set; }
      [JsonProperty("taskId")] 
      public Guid TaskId { get; set; }
      [JsonProperty("createdTime")] 
      public long CreatedTime { get; set; }
   }
}
