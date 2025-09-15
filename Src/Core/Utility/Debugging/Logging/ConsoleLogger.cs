using Crestron.SimplSharp;
using LinkLynx.Core.Interfaces;

namespace LinkLynx.Core.Utility.Debugging.Logging
{
    /// <summary>
    /// The main logging class in the framework, useful if Crestron console logging changes or later more advanced logging is introduced.
    /// </summary>
    public class ConsoleLogger : ILogger
    {
        private ConsoleLogger() { }

        /// <summary>
        /// Creates a new instance of the console logger and returns it.
        /// </summary>
        /// <returns></returns>
        public ILogger Create() => new ConsoleLogger();

        /// <summary>
        /// Logs a message to the CrestronConsole.
        /// </summary>
        public void Log(string message) => CrestronConsole.PrintLine(message);
    }
}
