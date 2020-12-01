namespace SuperSimpleConductor.ConductorWorker.Configuration
{
   public class ConductorSettings
   {
      public int QueuePollingIntervalInSeconds { get; set; } = 5;
      public string TaskDomain { get; set; } = null;
   }
}
