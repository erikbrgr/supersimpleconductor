using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SuperSimpleConductor.ConductorClient;
using SuperSimpleConductor.ConductorClient.Models;
using SuperSimpleConductor.ConductorWorker;
using SuperSimpleConductor.ConductorWorker.WorkflowTasks;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnitTests.ConductorWorker
{
   [TestClass]
   public class WorkflowTaskCoordinatorTests
   {
      public WorkflowTaskCoordinator Coordinator { get; private set; }
      public Mock<ConductorApi> ConductorApi { get; private set; }

      private class TestTask : IWorkflowTask
      {
         public Task<ConductorTaskResult> Execute(ConductorTask conductorTask)
         {
            return Task.FromResult(new ConductorTaskResult());
         }
      }

      [TestInitialize]
      public void Setup()
      {
         var testTask = new TestTask();

         ConductorApi = new Mock<ConductorApi>();
         ConductorApi.Setup(x => x.GetQueue()).Returns(Task.FromResult(new Dictionary<string, int>() { { "TestTask", 1 } }));
         ConductorApi.Setup(x => x.GetTask("TestTask", null)).Returns(Task.FromResult(new ConductorTask()));

         var serviceProvider = new Mock<IServiceProvider>();
         serviceProvider.Setup(x => x.GetService(typeof(TestTask))).Returns(testTask);

         Coordinator = new WorkflowTaskCoordinator(ConductorApi.Object, serviceProvider.Object, new TestLogger<WorkflowTaskCoordinator>());
         Coordinator.RegisterWorkflowTask(testTask);   
      }

      [TestMethod]
      public async Task SendAcknowledgementShouldNotBeCalled()
      {
         await Coordinator.PollConductorQueue(null, false);

         ConductorApi.Verify(x => x.SendAcknowledgement(It.IsAny<Guid>()), Times.Never());
      }

      [TestMethod]
      public async Task SendAcknowledgementShouldBeCalled()
      {
         await Coordinator.PollConductorQueue(null, true);

         ConductorApi.Verify(x => x.SendAcknowledgement(It.IsAny<Guid>()), Times.Once());
      }
   }
}