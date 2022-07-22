using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Ordering.Application.Helpers.Logging
{
    public static class UnobtrusiveLoggingHelper
    {
        public static void Log<T>(
            ILogger<T> logger,
            string message,
            LogLevel logLevel = LogLevel.Information,
            Exception exception = default,
            EventId eventId = default,
            params object[] args)
        {
            if (logger.IsEnabled(logLevel))
                Task.Run(() => logger.Log(logLevel, eventId, exception, message, args));
        }
    }
}