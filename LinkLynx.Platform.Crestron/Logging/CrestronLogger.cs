using Crestron.SimplSharp;
using LinkLynx.Core.Abstractions.Debugging.Logging;
using LinkLynx.Core.Debugging.Logging;
using System;
using System.Diagnostics;

namespace LinkLynx.Platform.Crestron.Logging
{
    internal class CrestronLogger : ILogger
    {
        public void Log(LogLevel level, string message, bool debugOnly = false, Exception exception = null)
        {
            #if !DEBUG
                if (debugOnly)
                    return;
            #endif

            CrestronConsole.PrintLine($"[{level}] {message}");

            if (exception != null)
            {
                CrestronConsole.PrintLine($"Exception: {exception.Message}");
                CrestronConsole.PrintLine($"Stack Trace: {exception.StackTrace}");
            }
        }

        public void LogException(Exception exception, bool debugOnly = false)
        {
            #if !DEBUG
                if (debugOnly)
                    return;
            #endif

            CrestronConsole.PrintLine($"Exception: {exception.Message}");
            CrestronConsole.PrintLine($"Stack Trace: {exception.StackTrace}");
        }
    }
}
