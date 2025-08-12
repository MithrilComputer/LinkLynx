using Crestron.SimplSharp;
using Crestron.SimplSharpPro;
using LinkLynx.Core.Logic.Pages;
using LinkLynx.Core.Utility.Dispatchers.Signals;
using LinkLynx.Core.Utility.Helpers;
using System;

namespace LinkLynx.Core.Utility.Dispatchers
{
    /// <summary>
    /// The DispatcherHelper class provides a unified interface for managing different types of signal dispatchers.
    /// </summary>
    internal static class DispatcherHelper
    {
        /// <summary>
        /// The method to add a join ID and its corresponding action to the dispatcher.
        /// </summary>
        /// <param name="join">The join ID Key.</param>
        /// <param name="action">The action that will be bound to the key.</param>
        /// <returns>True if the join ID was added, false if it already exists.</returns>
        /// <remarks>Ensure the enum has one of these in the name to be parsed correctly "Digital", "Analog" or "Serial". 
        /// It is used to determine the signal type.</remarks>
        internal static bool AddToDispatcher(Enum join, Action<PageLogicBase, SigEventArgs> action)
        {
            eSigType signalType = EnumHelper.GetSignalTypeFromEnum(join);
            uint joinId = Convert.ToUInt32(join);

            switch (signalType)
            {
                case eSigType.Bool:
                    return DigitalDispatcher.AddToDispatcher(joinId, action);
                case eSigType.UShort:
                    return AnalogDispatcher.AddToDispatcher(joinId, action);
                case eSigType.String:
                    return SerialDispatcher.AddToDispatcher(joinId, action);
                default:
                    throw new Exception("[DispatcherHelper] Incorrect Enum Value Passed when attempting to add a new logic join.");
            }
        }

        /// <summary>
        /// Checks if the dispatcher contains a specific join ID.
        /// </summary>
        /// <param name="signalType">The type of signal to check.</param>
        /// <param name="joinId">The join id associated with the signal.</param>
        /// <returns>If the dispatcher contains the key</returns>
        internal static bool CheckIfDispatcherContainsKey(eSigType signalType, uint joinId)
        {
            switch (signalType)
            {
                case eSigType.Bool:
                    return DigitalDispatcher.CheckIfDispatcherContainsKey(joinId);
                case eSigType.UShort:
                    return AnalogDispatcher.CheckIfDispatcherContainsKey(joinId);
                case eSigType.String:
                    return SerialDispatcher.CheckIfDispatcherContainsKey(joinId);
                default:
                    CrestronConsole.PrintLine($"[DispatcherHelper] Unsupported signal type: {signalType}, with a Join of {joinId}");
                    return false;
            }
        }

        /// <summary>
        /// Gets the action associated with a specific join ID.
        /// </summary>
        /// <param name="signalType">The type of signal to get the action from.</param>
        /// <param name="joinId">The join id of the action.</param>
        /// <returns>The action associated with a specific join ID, Null if not found.</returns>
        internal static Action<PageLogicBase, SigEventArgs> GetDispatcherActionFromKey(eSigType signalType, uint joinId)
        {
            switch (signalType)
            {
                case eSigType.Bool:
                    return DigitalDispatcher.GetActionFromKey(joinId);
                case eSigType.UShort:
                    return AnalogDispatcher.GetActionFromKey(joinId);
                case eSigType.String:
                    return SerialDispatcher.GetActionFromKey(joinId);
                default:
                    throw new FormatException("[DispatcherHelper] Input Enum Has Incorrect Formatting");
            }
        }

        /// <summary>
        /// Clears all the method dispatchers. Use only at system shutdown.
        /// </summary>
        internal static void Clear()
        {
            DigitalDispatcher.Clear();
            AnalogDispatcher.Clear();
            SerialDispatcher.Clear();
        }
    }
}
