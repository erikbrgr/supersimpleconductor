using Microsoft.Extensions.DependencyInjection;
using Refit;
using System;

namespace SuperSimpleConductor.ConductorClient
{
   public static class DependencyInjection
   {
      /// <summary>
      /// Adds the Conductor client.
      /// If the Conductor api address is not specified, the worker will use 
      /// the environment variable CONDUCTOR_API_ADDR to connect to the Conductor api.
      /// </summary>
      public static IServiceCollection AddConductorClient(this IServiceCollection services, Uri? conductorApiUri = null)
      {
         if (conductorApiUri == null)
         {
            var conductorApiAddress = Environment.GetEnvironmentVariable("CONDUCTOR_API_ADDR");
            if (conductorApiAddress == null)
               throw new ApplicationException("No Conductor api address specified or CONDUCTOR_API_ADDR not set");

            conductorApiUri = new Uri(conductorApiAddress);
         }

         var settings = new RefitSettings();
         // Inject a custom logger so we can see the payloads sent to Conductor
         settings.HttpMessageHandlerFactory = () => new HttpLoggingHandler();
         services.AddRefitClient<ConductorApi>(settings)
                 .ConfigureHttpClient(c => c.BaseAddress = conductorApiUri);

         return services;
      }
   }

   
}
