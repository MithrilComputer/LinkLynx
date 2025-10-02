using Crestron.SimplSharp;
using LinkLynx.Core.Interfaces.Utility.Debugging.Logging;
using System;

namespace LinkLynx.Implementations.Utility.Debugging.Logging
{
    /// <summary>
    /// The main logging class in the framework, useful if Crestron console logging changes or later more advanced logging is introduced.
    /// </summary>
    public sealed class ConsoleLogger : ILogger
    {
        private readonly ICrestronConsole console;

        /// <summary>
        /// The class constructor
        /// </summary>
        public ConsoleLogger(ICrestronConsole console) 
        {
            this.console = console;
        }

        /// <summary>
        /// Logs a message to the CrestronConsole.
        /// </summary>
        public void Log(string message) => console.PrintLine(message);
    }
}
