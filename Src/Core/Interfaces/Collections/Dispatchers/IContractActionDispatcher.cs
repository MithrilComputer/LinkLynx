using LinkLynx.Core.CrestronWrappers;
using LinkLynx.Implementations.Collections.Pages.Logic;

namespace LinkLynx.Core.Interfaces.Collections.Dispatchers
{
    /// <summary>
    /// The dispatcher for contract-based signals in the application.
    /// </summary>
    public interface IContractActionDispatcher
    {
        /// <summary>
        /// The method to add a Contract and its corresponding action to the dispatcher.
        /// </summary>
        /// <param name="contractName">The Contract Name Key</param>
        /// <param name="action">The action that is bound to the key</param>
        bool TryAdd(string contractName, Action<PageLogicScript, SignalEventData> action);

        /// <summary>
        /// Checks if the dispatcher contains a specific Contract.
        /// </summary>
        /// <param name="contractName">The key to be checked</param>
        bool Contains(string contractName);

        /// <summary>
        /// Gets the action associated with a specific join ID.
        /// </summary>
        /// <param name="contractName">The Id to get the action from</param>
        /// <returns>The action associated with the key, null if they key was not found</returns>
        Action<PageLogicScript, SignalEventData> Get(string contractName);
    }
}
