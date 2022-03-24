using System;
using SuperSimpleConductor.ConductorWorker;
using SuperSimpleConductor.ConductorClient;
using Microsoft.Extensions.Logging;
using Refit;

namespace TestWorker
{
   public class ConductorApiExceptionHandler : IConductorApiExceptionHandler
   {
      public ILogger<ConductorApiExceptionHandler> Logger { get; }

      public ConductorApiExceptionHandler(
          ILogger<ConductorApiExceptionHandler> logger
     ){
         Logger = logger;
      }

      public ConductorApi HandleException(Exception exception, ConductorApi conductorApi)
      {
         Logger.LogError("Error calling Conductor api");

         try
         {
            Logger.LogInformation("Attempting to recover..");

            // Simulate api discovery
            var conductorApiUrl = "http://localhost:8080/api";

            conductorApi = RestService.For<ConductorApi>(conductorApiUrl);

            Logger.LogInformation("New Conductor api address found: {conductorApi}", conductorApiUrl);

            return conductorApi;
         }
         catch (Exception ex)
         {
            Logger.LogError(ex, "Unable to recover");
            return null;
         }
      }
   }
}
