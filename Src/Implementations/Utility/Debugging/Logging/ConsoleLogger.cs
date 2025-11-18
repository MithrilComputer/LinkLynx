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
        /// <summary>
        /// The class constructor
        /// </summary>
        public ConsoleLogger() 
        {

        }

        /// <inheritdoc/>
        public List<string> Messages { get; } = new List<string>();

        /// <summary>
        /// Logs a message to the CrestronConsole.
        /// </summary>
        public void Log(string message)
        {
            CrestronConsole.PrintLine(message);

            Messages.Add(message);
        }
    }
}
