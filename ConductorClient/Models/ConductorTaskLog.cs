using System;

namespace SuperSimpleConductor.ConductorClient.Models
{
   public class ConductorTaskLog
   {
      public string Log { get; set; }
      public Guid TaskId { get; set; }
      public long CreatedTime { get; set; }
   }
}
