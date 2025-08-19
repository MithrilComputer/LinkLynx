using Crestron.SimplSharp;

namespace LinkLynx.Core.Utility.Debugging.Logging
{
    /// <summary>
    /// The main logging class in the framework, useful if Crestron console logging changes or later more advanced logging is introduced.
    /// </summary>
    public static class ConsoleLogger
    {
        /// <summary>
        /// Logs a message to the CrestronConsole.
        /// </summary>
        public static void Log(string message) => CrestronConsole.PrintLine(message);
    }
}
