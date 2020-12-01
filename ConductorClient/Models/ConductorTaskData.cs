using System.Collections.Generic;

namespace SuperSimpleConductor.ConductorClient.Models
{
   public class ConductorTaskData : Dictionary<string, object>
   {
      public ConductorTaskData() : base()
      {

      }

      public ConductorTaskData(IDictionary<string, object> source) : base(source)
      {

      }
   }
}