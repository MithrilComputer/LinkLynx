using Crestron.SimplSharp;
using LinkLynx.Core.Abstractions.Debugging.Logging;
using LinkLynx.Core.Debugging.Logging;
using System;
using System.Diagnostics;

namespace LinkLynx.Platform.Crestron.Logging
{
    internal class CrestronContextLogger<T> : IContextLogger<T>
    {
        public void Log(LogLevel level, string message, bool debugOnly = false, Exception exception = null)
        {
            #if !DEBUG
                if (debugOnly)
                    return;
            #endif

            string context = typeof(T).Name;

            CrestronConsole.PrintLine($"[{level}] [{context}] {message}");

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

            string context = typeof(T).Name;

            CrestronConsole.PrintLine($"[{context}] Exception: {exception.Message}");
            CrestronConsole.PrintLine($"[{context}] Stack Trace: {exception.StackTrace}");
        }
    }
}
