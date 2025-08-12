using Crestron.SimplSharp;
using Crestron.SimplSharpPro;
using LinkLynx.Core.Logic.Pages;
using System;
using System.Collections.Generic;

namespace LinkLynx.Core.Utility.Dispatchers.Signals
{
    internal static class AnalogDispatcher
    {
        /// <summary>
        /// A dictionary that maps digital join IDs to their corresponding actions.
        /// </summary>
        private static readonly Dictionary<uint, Action<PageLogicBase, SigEventArgs>> dispatcher = new Dictionary<uint, Action<PageLogicBase, SigEventArgs>>();

        /// <summary>
        /// The method to add a join ID and its corresponding action to the dispatcher.
        /// </summary>
        /// <param name="joinId">The join ID Key</param>
        /// <param name="action">The action that is bound to the key</param>
        /// <returns>True if the join ID was added, false if it already exists.</returns>
        internal static bool AddToDispatcher(uint joinId, Action<PageLogicBase, SigEventArgs> action)
        {
            if (!dispatcher.ContainsKey(joinId))
            {
                dispatcher.Add(joinId, action);
                CrestronConsole.PrintLine($"[AnalogDispatcher] Join ID {joinId} bound to {action.Method.Name} in the dispatcher.");
                return true;
            }
            else
            {
                CrestronConsole.PrintLine($"[AnalogDispatcher] Join ID {joinId} already exists in the dispatcher for {action.Method.Name}.");
                return false;
            }
        }

        /// <summary>
        /// Checks if the dispatcher contains a specific join ID.
        /// </summary>
        /// <param name="joinId">The key to be checked</param>
        internal static bool CheckIfDispatcherContainsKey(uint joinId)
        {
            if (dispatcher.ContainsKey(joinId))
            {
                CrestronConsole.PrintLine($"[AnalogDispatcher] Join ID {joinId} exists in the dispatcher.");
                return true;
            }
            else
            {
                CrestronConsole.PrintLine($"[AnalogDispatcher] Join ID {joinId} does not exist in the dispatcher.");
                return false;
            }
        }

        /// <summary>
        /// Gets the action associated with a specific join ID.
        /// </summary>
        /// <param name="joinId">The Id to get the action from</param>
        /// <returns>The action associated with the key</returns>
        internal static Action<PageLogicBase, SigEventArgs> GetActionFromKey(uint joinId)
        {
            if (dispatcher.TryGetValue(joinId, out var action))
            {
                CrestronConsole.PrintLine($"[AnalogDispatcher] Found action {action.Method.Name} for join ID {joinId}.");
                return action;
            }
            else
            {
                CrestronConsole.PrintLine($"[AnalogDispatcher] No action found for join ID {joinId}.");
                return null;
            }
        }

        /// <summary>
        /// Clears the dispatcher entries. Only use at system shutdown.
        /// </summary>
        internal static void Clear()
        {
            dispatcher.Clear();
        }
    }
}
