using LinkLynx.Core.CrestronWrappers;
using LinkLynx.Core.Interfaces.Collections.Dispatchers;
using LinkLynx.Core.Interfaces.Utility.Debugging.Logging;
using LinkLynx.Implementations.Collections.Pages.Logic;

namespace LinkLynx.Implementations.Collections.Dispatchers.SimpleSignals
{
    /// <summary>
    /// Dispatcher for analog signals in the application.
    /// </summary>
    public sealed class AnalogDispatcher : IAnalogJoinDispatcher, IDisposable
    {
        private readonly ILogger consoleLogger;

        /// <summary>
        /// Class constructor.
        /// </summary>
        public AnalogDispatcher(ILogger consoleLogger)
        {
            this.consoleLogger = consoleLogger;
        }

        /// <summary>
        /// How many items are in the dispatcher.
        /// </summary>
        public int Count => dispatcher.Count;

        /// <summary>
        /// A dictionary that maps digital join IDs to their corresponding actions.
        /// </summary>
        private readonly Dictionary<uint, Action<PageLogicScript, SignalEventData>> dispatcher = new Dictionary<uint, Action<PageLogicScript, SignalEventData>>();

        /// <summary>
        /// The method to add a join ID and its corresponding action to the dispatcher.
        /// </summary>
        /// <param name="joinId">The join ID Key</param>
        /// <param name="action">The action that is bound to the key</param>
        /// <returns>True if the join ID was added, false if it already exists.</returns>
        public bool TryAdd(uint joinId, Action<PageLogicScript, SignalEventData> action)
        {
            if (!dispatcher.ContainsKey(joinId))
            {
                dispatcher.Add(joinId, action);
                consoleLogger.Log($"[AnalogDispatcher] Join ID {joinId} bound to {action.Method.Name} in the dispatcher.");
                return true;
            }
            else
            {
                consoleLogger.Log($"[AnalogDispatcher] Join ID {joinId} already exists in the dispatcher for {action.Method.Name}.");
                return false;
            }
        }

        /// <summary>
        /// Checks if the dispatcher contains a specific join ID.
        /// </summary>
        /// <param name="joinId">The key to be checked</param>
        public bool Contains(uint joinId)
        {
            if (dispatcher.ContainsKey(joinId))
            {
                consoleLogger.Log($"[AnalogDispatcher] Join ID {joinId} exists in the dispatcher.");
                return true;
            }
            else
            {
                consoleLogger.Log($"[AnalogDispatcher] Join ID {joinId} does not exist in the dispatcher.");
                return false;
            }
        }

        /// <summary>
        /// Gets the action associated with a specific join ID.
        /// </summary>
        /// <param name="joinId">The Id to get the action from</param>
        /// <returns>The action associated with the key, null if they key was not found</returns>
        public Action<PageLogicScript, SignalEventData> Get(uint joinId)
        {
            if (dispatcher.TryGetValue(joinId, out var action))
            {
                consoleLogger.Log($"[AnalogDispatcher] Found action {action.Method.Name} for join ID {joinId}.");
                return action;
            }
            else
            {
                consoleLogger.Log($"[AnalogDispatcher] No action found for join ID {joinId}.");
                return null;
            }
        }

        /// <summary>
        /// Clears the dispatcher entries. Only use at system shutdown.
        /// </summary>
        public void Dispose()
        {
            dispatcher.Clear();
        }
    }
}
