using LinkLynx.Core.Logic.Pages;
using LinkLynx.Core.CrestronWrappers;

namespace LinkLynx.Core.Interfaces.Utility.Dispatching
{
    /// <summary>
    /// The IJoinDispatcher interface defines a contract for managing a collection of actions
    /// </summary>
    public interface IJoinDispatcher
    {
        /// <summary>
        /// Adds an action to the dispatcher associated with a specific join.
        /// </summary>
        /// <returns>If the join was sucsessfully added</returns>
        bool AddToDispatcher(Enum join, Action<PageLogicScript, SignalEventData> action);

        /// <summary>
        /// Checks if the dispatcher contains a key for the given signal type and join ID.
        /// </summary>
        bool CheckIfDispatcherContainsKey(Signals.SigType signalType, uint joinId);

        /// <summary>
        /// Gets the action associated with the specified signal type and join ID.
        /// </summary>
        Action<PageLogicScript, SignalEventData> GetDispatcherActionFromKey(Signals.SigType signalType, uint joinId);
    }
}
