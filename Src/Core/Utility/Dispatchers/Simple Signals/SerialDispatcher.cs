using Crestron.SimplSharpPro;
using LinkLynx.Core.Logic.Pages;
using LinkLynx.Core.Utility.Debugging.Logging;
using System;
using System.Collections.Generic;

namespace LinkLynx.Core.Utility.Dispatchers.Signals
{
    /// <summary>
    /// Dispatcher for serial signals in the application.
    /// </summary>
    internal sealed class SerialDispatcher
    {
        /// <summary>
        /// The singleton instance of the class.
        /// </summary>
        private static readonly SerialDispatcher instance = new SerialDispatcher();

        /// <summary>
        /// The singleton instance of the class.
        /// </summary>
        public static SerialDispatcher Instance => instance;

        /// <summary>
        /// Class constructor.
        /// </summary>
        internal SerialDispatcher() { }

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
        internal bool AddToDispatcher(uint joinId, Action<PageLogicBase, SigEventArgs> action)
        {
            if (!dispatcher.ContainsKey(joinId))
            {
                dispatcher.Add(joinId, action);
                ConsoleLogger.Log($"[SerialDispatcher] Join ID {joinId} bound to {action.Method.Name} in the dispatcher.");
                return true;
            }
            else
            {
                ConsoleLogger.Log($"[SerialDispatcher] Join ID {joinId} already exists in the dispatcher for {action.Method.Name}.");
                return false;
            }
        }

        /// <summary>
        /// Checks if the dispatcher contains a specific join ID.
        /// </summary>
        /// <param name="joinId">The key to be checked</param>
        internal bool CheckIfDispatcherContainsKey(uint joinId)
        {
            if (dispatcher.ContainsKey(joinId))
            {
                ConsoleLogger.Log($"[SerialDispatcher] Join ID {joinId} exists in the dispatcher.");
                return true;
            }
            else
            {
                ConsoleLogger.Log($"[SerialDispatcher] Join ID {joinId} does not exist in the dispatcher.");
                return false;
            }
        }

        /// <summary>
        /// Gets the action associated with a specific join ID.
        /// </summary>
        /// <param name="joinId">The Id to get the action from</param>
        /// <returns>The action associated with the key</returns>
        internal Action<PageLogicBase, SigEventArgs> GetActionFromKey(uint joinId)
        {
            if (dispatcher.TryGetValue(joinId, out var action))
            {
                ConsoleLogger.Log($"[SerialDispatcher] Found action {action.Method.Name} for join ID {joinId}.");
                return action;
            }
            else
            {
                ConsoleLogger.Log($"[SerialDispatcher] No action found for join ID {joinId}.");
                return null;
            }
        }

        /// <summary>
        /// Clears the dispatcher entries. Only use at system shutdown.
        /// </summary>
        internal void Clear()
        {
            dispatcher.Clear();
        }
    }
}
