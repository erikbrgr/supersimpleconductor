using Microsoft.Extensions.Hosting;
using System;
using SuperSimpleConductor.ConductorWorker;
using SuperSimpleConductor.ConductorClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Refit;

namespace TestWorker
{
   public class Program
   {
      public static void Main(string[] args)
      {
         CreateHostBuilder(args).Build()
                                .RegisterWorkflowTasks()
                                .Run();
      }

      public static IHostBuilder CreateHostBuilder(string[] args) =>
                Host.CreateDefaultBuilder(args)
                    .ConfigureServices((hostContext, services) =>
                    {
                       services.AddConductorWorker(hostContext.Configuration, new Uri("http://localhost:8080/"));

                       //services.AddTransient<IConductorApiExceptionHandler, ConductorApiExceptionHandler>();

                       //services.AddWorkflowTasks("<assembly containing your worker tasks>");
                    });
   }

   public class ConductorApiExceptionHandler : IConductorApiExceptionHandler
   {
      public ILogger<ConductorApiExceptionHandler> Logger { get; }

      public ConductorApiExceptionHandler(ILogger<ConductorApiExceptionHandler> logger)
      {
         Logger = logger;
      }

      public ConductorApi HandleException(Exception exception, ConductorApi conductorApi)
      {
         Logger.LogError("Error calling Conductor api");

         try
         {
            Logger.LogInformation("Attempting to recover..");

            // Simulate api discovery
            var conductorApiUrl = "http://localhost:8080";

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
