namespace LinkLynx.Core.Interfaces.Utility.Debugging.Logging
{   
    /// <summary>
    /// Public interface for logging to the console.
    /// </summary>
    public interface ILogger 
    {
        /// <summary>
        /// The list of all sent messages
        /// </summary>
        List<string> Messages { get; }

        /// <summary>
        /// Logs to the Crestron console.
        /// </summary>
        /// <param name="message"></param>
        void Log(string message); 
    }
}
