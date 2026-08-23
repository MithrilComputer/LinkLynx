using LinkLynx.Core.Debugging.Logging;
using System;

namespace LinkLynx.Core.Abstractions.Debugging.Logging
{
    public interface ILogger
    {
        void Log(
            LogLevel level,
            string message,
            bool debugOnly = false,
            Exception exception = null);

        void LogException(
            Exception exception,
            bool debugOnly = false);
    }
}
