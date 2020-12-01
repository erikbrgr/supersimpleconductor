# SuperSimpleConductor

SuperSimpleConductor allows for the quick and easy creation of a .NET Core Worker Service that polls a [Netflix Conductor](https://netflix.github.io/conductor/) instance for tasks and executes them when available.

There are 2 packages in this solution.

## ConductorClient

This is a very simple client built using Refit. The implemented API functions are limited to what is necessary for polling Netflix Conductor.

## ConductorWorker

Again, a very simple implementation of a BackgroundService class to poll a Netflix Conductor instance and execute the tasks registered.

## Sample Code

_Coming soon._

In the meanwhile, a good start is to just use ConductorWorker in the default `worker` template:

```
namespace WorkerService1
{
   public class Program
   {
      public static void Main(string[] args)
      {
         CreateHostBuilder(args).Build().Run();
      }

      public static IHostBuilder CreateHostBuilder(string[] args) =>
          Host.CreateDefaultBuilder(args)
              .ConfigureServices((hostContext, services) =>
              {
                 services.AddHostedService<ConductorWorker>();
              });
   }
}
```

### Configuration

By default, ConductorWorker polls Conductor every 5 seconds and does not use a [Task Domain](https://netflix.github.io/conductor/configuration/taskdomains/). The following snippet from `appsettings.json` configures polling to be every 3 minutes for the domain `poller`:

```
   "ConductorSettings": {
      "QueuePollingIntervalInSeconds": 180,
      "TaskDomain": "poller"
   }
```
