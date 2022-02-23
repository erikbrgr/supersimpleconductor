using Microsoft.Extensions.Hosting;
using System;
using SuperSimpleConductor.ConductorWorker;

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
         Host.CreateDefaultBuilder(args).ConfigureServices((hostContext, services) =>
            {
               services.AddConductorWorker(hostContext.Configuration, new Uri("http://localhost:8080/api"));

               // services.AddTransient<IConductorApiExceptionHandler, ConductorApiExceptionHandler>();

               services.AddWorkflowTasks("TestWorker");
            });
   }
}
