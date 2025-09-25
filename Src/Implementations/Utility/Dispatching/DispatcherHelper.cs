using Crestron.SimplSharpPro;
using LinkLynx.Core.Logic.Pages;
using LinkLynx.Core.Utility.Helpers;
using LinkLynx.Interfaces.Collections.Dispatchers;
using LinkLynx.Interfaces.Utility.Dispatching;
using System;
using LinkLynx.Interfaces.Debugging;

namespace LinkLynx.Core.Utility.Dispatchers
{
    /// <summary>
    /// The DispatcherHelper class provides a unified interface for managing different types of signal dispatchers.
    /// </summary>
    internal class DispatcherHelper : IJoinDispatcher
    {
        private readonly IDigitalJoinDispatcher digitalDispatcher;
        private readonly IAnalogJoinDispatcher analogDispatcher;
        private readonly ISerialJoinDispatcher serialDispatcher;
        private readonly ILogger consoleLogger;

        public DispatcherHelper(IDigitalJoinDispatcher digitalDispatcher, IAnalogJoinDispatcher analogDispatcher, ISerialJoinDispatcher serialDispatcher, ) 
        {
            this.digitalDispatcher = digitalDispatcher;
            this.analogDispatcher = analogDispatcher;
            this.serialDispatcher = serialDispatcher;
        }


        /// <summary>
        /// The method to add a join ID and its corresponding action to the dispatcher.
        /// </summary>
        /// <param name="join">The join ID Key.</param>
        /// <param name="action">The action that will be bound to the key.</param>
        /// <returns>True if the join ID was added, false if it already exists.</returns>
        /// <remarks>Ensure the enum has one of these in the name to be parsed correctly "Digital", "Analog" or "Serial". 
        /// It is used to determine the signal type.</remarks>
        public bool AddToDispatcher(Enum join, Action<PageLogicBase, SigEventArgs> action)
        {
            eSigType signalType = EnumHelper.GetSignalTypeFromEnum(join);
            uint joinId = Convert.ToUInt32(join);

            consoleLogger.Log($"[DispatcherHelper] Adding Join {joinId} of type {signalType} to dispatcher.");

            switch (signalType)
            {
                case eSigType.Bool:
                    return digitalDispatcher.TryAdd(joinId, action);
                case eSigType.UShort:
                    return analogDispatcher.TryAdd(joinId, action);
                case eSigType.String:
                    return serialDispatcher.TryAdd(joinId, action);
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
        public bool CheckIfDispatcherContainsKey(eSigType signalType, uint joinId)
        {
            switch (signalType)
            {
                case eSigType.Bool:
                    return digitalDispatcher.Contains(joinId);
                case eSigType.UShort:
                    return analogDispatcher.Contains(joinId);
                case eSigType.String:
                    return serialDispatcher.Contains(joinId);
                default:
                    consoleLogger.Log($"[DispatcherHelper] Unsupported signal type: {signalType}, with a Join of {joinId}");
                    return false;
            }
        }

        /// <summary>
        /// Gets the action associated with a specific join ID.
        /// </summary>
        /// <param name="signalType">The type of signal to get the action from.</param>
        /// <param name="joinId">The join id of the action.</param>
        /// <returns>The action associated with a specific join ID, Null if not found.</returns>
        public Action<PageLogicBase, SigEventArgs> GetDispatcherActionFromKey(eSigType signalType, uint joinId)
        {
            switch (signalType)
            {
                case eSigType.Bool:
                    return digitalDispatcher.Get(joinId);
                case eSigType.UShort:
                    return analogDispatcher.Get(joinId);
                case eSigType.String:
                    return serialDispatcher.Get(joinId);
                default:
                    throw new FormatException("[DispatcherHelper] Input Enum Has Incorrect Formatting");
            }
        }

    }
}
