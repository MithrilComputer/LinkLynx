using LinkLynx.Core.CrestronWrappers;
using LinkLynx.Core.Interfaces.Collections.Dispatchers;
using LinkLynx.Core.Interfaces.Utility.Debugging.Logging;
using LinkLynx.Implementations.Collections.Pages.Logic;

namespace LinkLynx.Implementations.Collections.Dispatchers.ContractSignals
{
    /// <summary>
    /// The dispatcher for contract-based signals in the application.
    /// </summary>
    public sealed class ContractActionDispatcher : IContractActionDispatcher, IDisposable
    {
        private readonly ILogger logger;

        /// <summary>
        /// A dictionary that maps digital join IDs to their corresponding actions.
        /// </summary>
        private readonly Dictionary<string, Action<PageLogicScript, SignalEventData>> dispatcher = new Dictionary<string, Action<PageLogicScript, SignalEventData>>();

        /// <summary>
        /// Class constructor.
        /// </summary>
        public ContractActionDispatcher(ILogger logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// The method to add a Contract and its corresponding action to the dispatcher.
        /// </summary>
        /// <param name="contractName">The Contract Name Key</param>
        /// <param name="action">The action that is bound to the key</param>
        public bool TryAdd(string contractName, Action<PageLogicScript, SignalEventData> action)
        {
            if (!dispatcher.ContainsKey(contractName))
            {
                dispatcher.Add(contractName, action);
                logger.Log($"[ContractActionDispatcher] Warning: Join ID {contractName} bound to {action.Method.Name} in the dispatcher.");
                return true;
            }
            else
            {
                logger.Log($"[ContractActionDispatcher] Warning: Join ID {contractName} already exists in the dispatcher for {action.Method.Name}.");
                return false;
            }
        }

        /// <summary>
        /// Checks if the dispatcher contains a specific Contract.
        /// </summary>
        /// <param name="contractName">The key to be checked</param>
        public bool Contains(string contractName)
        {
            return dispatcher.ContainsKey(contractName);
        }

        /// <summary>
        /// Gets the action associated with a specific join ID.
        /// </summary>
        /// <param name="contractName">The Id to get the action from</param>
        /// <returns>The action associated with the key, null if they key was not found</returns>
        public Action<PageLogicScript, SignalEventData> Get(string contractName)
        {
            if (dispatcher.TryGetValue(contractName, out var action))
            {
                return action;
            }
            else
            {
                logger.Log($"[AnalogDispatcher] No action found for join ID {contractName}.");
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
