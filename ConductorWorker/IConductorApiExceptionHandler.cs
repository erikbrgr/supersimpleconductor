using Refit;
using SuperSimpleConductor.ConductorClient;
using System;

namespace SuperSimpleConductor.ConductorWorker
{
   public interface IConductorApiExceptionHandler
   {
      /// <summary>
      /// Handle the Exception.
      /// 
      /// </summary>
      /// <param name="exception">The exception to handle.</param>
      /// <param name="conductorApi">The instance of the ConductorApi that raised the exception.</param>
      /// <returns>A new instance of the ConductorApi, or <c>null</c> if the current instance should remain unchanged.</returns>
      ConductorApi HandleException(Exception exception, ConductorApi conductorApi);
   }
}