namespace LinkLynx.Core.Interfaces
{   
    /// <summary>
    /// Public interface for logging to the console.
    /// </summary>
    public interface ILogger 
    {
        /// <summary>
        /// Logs to the Crestron console.
        /// </summary>
        /// <param name="message"></param>
        void Log(string message); 
    }
}
