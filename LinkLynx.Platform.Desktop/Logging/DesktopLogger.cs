using LinkLynx.Core.Abstractions.Debugging.Logging;
using LinkLynx.Core.Debugging.Logging;
using System.Diagnostics;

namespace LinkLynx.Platform.Desktop.Logging
{
    internal class DesktopLogger : ILogger
    {
        public void Log(LogLevel level, string message, bool debugOnly = false, Exception? exception = null)
        {
            #if !DEBUG
                if (debugOnly)
                    return;
            #endif

            Debug.WriteLine($"[{level}] {message}");

            if (exception != null)
            {
                Debug.WriteLine($"Exception: {exception.Message}");
                Debug.WriteLine($"Stack Trace: {exception.StackTrace}");
            }
        }

        public void LogException(Exception exception, bool debugOnly = false)
        {
            #if !DEBUG
                if (debugOnly)
                    return;
            #endif

            Debug.WriteLine($"Exception: {exception.Message}");
            Debug.WriteLine($"Stack Trace: {exception.StackTrace}");
        }
    }
}
