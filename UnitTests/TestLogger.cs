using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;

namespace UnitTests
{
   public class TestLogger<T> : ILogger<T>, IDisposable
   {
      public IDisposable BeginScope<TState>(TState state)
      {
         return this;
      }

      public bool IsEnabled(LogLevel logLevel)
      {
         return true;
      }

      public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
      {
         var message = $"{DateTime.Now.ToString("HH:mm:ss.fff")} | {logLevel} | {state.ToString()} | {Environment.NewLine}{exception}";

         Debug.WriteLine(message);
         Console.WriteLine(message);
      }

      public void Dispose() { }
   }
}
