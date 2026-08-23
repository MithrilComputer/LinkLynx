using LinkLynx.Core.Abstractions.Debugging.Logging;
using LinkLynx.Core.Debugging.Logging;
using System.Diagnostics;

namespace LinkLynx.Platform.Desktop.Logging
{
    internal class DesktopContextLogger<T> : IContextLogger<T>
    {
        public void Log(LogLevel level, string message, bool debugOnly = false, Exception? exception = null)
        {
            #if !DEBUG
                if (debugOnly)
                    return;
            #endif

            string context = typeof(T).Name;

            Debug.WriteLine($"[{level}] [{context}] {message}");

            if (exception != null)
            {
                Debug.WriteLine($"[{context}] Exception: {exception.Message}");
                Debug.WriteLine($"[{context}] Stack Trace: {exception.StackTrace}");
            }
        }

        public void LogException(Exception exception, bool debugOnly = false)
        {
            #if !DEBUG
                if (debugOnly)
                    return;
            #endif

            string context = typeof(T).Name;

            Debug.WriteLine($"[{context}] Exception: {exception.Message}");
            Debug.WriteLine($"[{context}] Stack Trace: {exception.StackTrace}");
        }
    }
}
