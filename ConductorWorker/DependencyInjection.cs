using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using SuperSimpleConductor.ConductorClient;
using SuperSimpleConductor.ConductorWorker.Configuration;
using SuperSimpleConductor.ConductorWorker.WorkflowTasks;
using System;
using System.Linq;
using System.Reflection;

namespace SuperSimpleConductor.ConductorWorker
{
   public static class DependencyInjection
   {
      /// <summary>
      /// Adds the Conductor worker as a background service. 
      /// If the Conductor api address is not specified, the worker will use 
      /// the environment variable CONDUCTOR_API_ADDR to connect to the Conductor api.
      /// </summary>
      public static IServiceCollection AddConductorWorker(this IServiceCollection services, IConfiguration configuration, Uri? conductorApiUri = null)
      {
         services.Configure<ConductorSettings>(configuration.GetSection("ConductorSettings"));

         services.AddConductorClient(conductorApiUri);

         services.AddSingleton<IWorkflowTaskCoordinator, WorkflowTaskCoordinator>();

         services.AddHostedService<ConductorWorker>();

         return services;
      }

      /// <summary>
      /// Registers workflow tasks defined in the specified assemblies
      /// </summary>
      public static IServiceCollection AddWorkflowTasks(this IServiceCollection services, params string[] workerTasksAssemblies)
      {
         var addWorkflowTaskMethod = typeof(DependencyInjection).GetMethod("AddWorkflowTask");

         foreach (var assembly in workerTasksAssemblies)
         {
            var workflowTaskAssembly = Assembly.Load(assembly);

            var workflowTaskTypes = workflowTaskAssembly.GetExportedTypes().Where(t => typeof(IWorkflowTask).IsAssignableFrom(t) && !t.IsAbstract);
            if (workflowTaskTypes.Count() == 0)
            {
               Log.Warning("No workflow task types found in assembly {Assembly}", assembly);
            }
            else
            {
               foreach (var workflowTaskType in workflowTaskTypes)
               {
                  Log.Debug("Adding workflow task type {WorkflowTaskType}", workflowTaskType.Name);

                  addWorkflowTaskMethod.MakeGenericMethod(workflowTaskType)
                                       .Invoke(null, new object[] { services });
               }
            }
         }

         return services;
      }

      /// <summary>
      /// Registers a workflow task.
      /// </summary>
      public static IServiceCollection AddWorkflowTask<T>(this IServiceCollection services) where T : IWorkflowTask
      {
         // Register the workflow task twice: once to be able to retrieve it by
         // interface, once to retrieve it by type.
         services.AddTransient(typeof(IWorkflowTask), typeof(T));
         services.AddTransient(typeof(T));

         return services;
      }

      /// <summary>
      /// Registers available workflow tasks (added using AddWorkflowTask) with the workflow task coordinator.
      /// </summary>
      public static IHost RegisterWorkflowTasks(this IHost host)
      {
         var serviceProvider = host.Services;
         var workflowTaskCoordinator = serviceProvider.GetService<IWorkflowTaskCoordinator>();

         if (workflowTaskCoordinator == null)
            throw new ApplicationException("Call AddConductorWorker before registering workflow tasks.");

         var workflowTasks = serviceProvider.GetServices<IWorkflowTask>();
         if (workflowTasks.Count() == 0)
         {
            Log.Warning("No workflow tasks were found. Register workflow tasks using AddWorkflowTasks");
         }
         else
         {
            foreach (var workflowTask in workflowTasks)
            {
               workflowTaskCoordinator.RegisterWorkflowTask(workflowTask);
            }
         }

         return host;
      }
   }
}
