### ⚠️ Breaking Change ⚠️

From version 0.3 of ConductorWorker, no acknowledgements (Ack) will be sent to Conductor when polling a task. This will break Conductor versions < 3.0. To re-enable Ack, use the `SendAck` configuration setting (see below).

# SuperSimpleConductor

SuperSimpleConductor allows for the quick and easy creation of a .NET Core Worker Service that polls a [Netflix Conductor](https://netflix.github.io/conductor/) instance for tasks and executes them when available.

There are 2 packages in this solution.

## ConductorClient

This is a very simple client built using Refit. The implemented API functions are limited to what is necessary for polling Netflix Conductor.

## ConductorWorker

Again, a very simple implementation of a BackgroundService class to poll a Netflix Conductor instance and execute the tasks registered.

## Getting Started

To get started, please read [Getting Started with Netflix Conductor in .NET using SuperSimpleConductor](https://betterprogramming.pub/getting-started-with-netflix-conductor-in-net-using-supersimpleconductor-ed8a02120c1).

### Configuration

By default, ConductorWorker polls Conductor every 5 seconds and does not use a [Task Domain](https://netflix.github.io/conductor/configuration/taskdomains/). The following snippet from `appsettings.json` configures polling to be every 3 minutes for the domain `poller`:

```json
   "ConductorSettings": {
      "QueuePollingIntervalInSeconds": 180,
      "TaskDomain": "poller"
   }
```

#### `SendAck`

In v3.0.0 of Conductor, the Ack API endpoint was deprecated and removed (see https://github.com/Netflix/conductor/issues/1623 for some details). The `SendAck` property allows you to turn calling the Ack API endpoint back on (it is `false` by default).

```json
   "ConductorSettings": {
      "SendAck": true
   }
```
