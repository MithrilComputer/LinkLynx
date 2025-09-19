using Crestron.SimplSharpPro;
using LinkLynx.Core.Logic.Pages;
using LinkLynx.Interfaces.Collections.Dispatchers;
using LinkLynx.Core.Utility.Debugging.Logging;
using System;
using System.Collections.Generic;

namespace LinkLynx.Core.Utility.Dispatchers.Signals
{
    /// <summary>
    /// Dispatcher for digital signals in the application.
    /// </summary>
    internal sealed class DigitalDispatcher : IDigitalJoinDispatcher
    {
        /// <summary>
        /// Creates a new instance of the DigitalDispatcher and passes it as an ILogicJoinDispatcher
        /// </summary>
        /// <returns></returns>
        public IDigitalJoinDispatcher Create() { return new DigitalDispatcher(); }

        /// <summary>
        /// Class constructor.
        /// </summary>
        private DigitalDispatcher() { }

        /// <summary>
        /// How many items are in the dispatcher.
        /// </summary>
        public int Count => dispatcher.Count;

        /// <summary>
        /// A dictionary that maps digital join IDs to their corresponding actions.
        /// </summary>
        private readonly Dictionary<uint, Action<PageLogicBase, SigEventArgs>> dispatcher = new Dictionary<uint, Action<PageLogicBase, SigEventArgs>>();

        /// <summary>
        /// The method to add a join ID and its corresponding action to the dispatcher.
        /// </summary>
        /// <param name="joinId">The join ID Key</param>
        /// <param name="action">The action that is bound to the key</param>
        /// <returns>True if the join ID was added, false if it already exists.</returns>
        public bool TryAddToDispatcher(uint joinId, Action<PageLogicBase, SigEventArgs> action)
        {
            if (!dispatcher.ContainsKey(joinId))
            {
                dispatcher.Add(joinId, action);
                ConsoleLogger.Log($"[DigitalDispatcher] Join ID {joinId} bound to {action.Method.Name} in the dispatcher.");
                return true;
            }
            else
            {
                ConsoleLogger.Log($"[DigitalDispatcher] Join ID {joinId} already exists in the dispatcher for {action.Method.Name}.");
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
                ConsoleLogger.Log($"[DigitalDispatcher] Join ID {joinId} exists in the dispatcher.");
                return true;
            }
            else
            {
                ConsoleLogger.Log($"[DigitalDispatcher] Join ID {joinId} does not exist in the dispatcher.");
                return false;
            }
        }

        /// <summary>
        /// Gets the action associated with a specific join ID.
        /// </summary>
        /// <param name="joinId">The Id to get the action from</param>
        /// <returns>The action associated with the key</returns>
        public Action<PageLogicBase, SigEventArgs> Get(uint joinId)
        {
            if (dispatcher.TryGetValue(joinId, out var action))
            {
                ConsoleLogger.Log($"[DigitalDispatcher] Found action {action.Method.Name} for join ID {joinId}.");
                return action;
            }
            else
            {
                ConsoleLogger.Log($"[DigitalDispatcher] No action found for join ID {joinId}.");
                return null;
            }
        }

        /// <summary>
        /// Clears the dispatcher entries. Only use at system shutdown.
        /// </summary>
        public void Clear()
        {
            dispatcher.Clear();
        }
    }
}
