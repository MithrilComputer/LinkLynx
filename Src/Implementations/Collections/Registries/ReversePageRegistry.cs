using Crestron.SimplSharpPro;
using LinkLynx.Core.Interfaces.Collections.Registries;
using LinkLynx.Core.Interfaces.Utility.Debugging.Logging;
using LinkLynx.Implementations.Utility.Helpers;
using System;
using System.Collections.Generic;

namespace LinkLynx.Implementations.Collections.Registries
{
    /// <summary>
    /// A special class that used to link logic joins to pages, good for reverse join searching.
    /// </summary>
    internal sealed class ReversePageRegistry : IReversePageRegistry, IDisposable
    {
        private readonly ILogger consoleLogger;
        private readonly EnumHelper enumHelper;

        /// <summary>
        /// Class constructor.
        /// </summary>
        public ReversePageRegistry(ILogger consoleLogger, EnumHelper enumHelper) 
        { 
            this.consoleLogger = consoleLogger;
            this.enumHelper = enumHelper;
        }

        private readonly Dictionary<uint, ushort> DigitalJoinPageMap = new Dictionary<uint, ushort>();
        private readonly Dictionary<uint, ushort> AnalogJoinPageMap = new Dictionary<uint, ushort>();
        private readonly Dictionary<uint, ushort> SerialJoinPageMap = new Dictionary<uint, ushort>();

        /// <summary>
        /// Gets a page ID given an input join ID and Join Type. Gives ushort.MaxValue if the key was not found.
        /// </summary>
        /// <param name="join">The join ID to search for the page.</param>
        /// <param name="type">The type of join associated with the key.</param>
        public ushort Get(uint join, eSigType type)
        {
            switch (type)
            {
                case eSigType.Bool:
                    if (DigitalJoinPageMap.TryGetValue(join, out ushort digitalPageID))
                    {
                        return digitalPageID;
                    }

                    consoleLogger.Log($"[ReversePageRegistry] Warning: Could not find the page associated with the signal key of '{join}' with a type of '{type}'");

                    break;

                case eSigType.UShort:
                    if (AnalogJoinPageMap.TryGetValue(join, out ushort analogPageID))
                    {
                        return analogPageID;
                    }

                    consoleLogger.Log($"[ReversePageRegistry] Warning: Could not find the page associated with the signal key of '{join}' with a type of '{type}'");

                    break;

                case eSigType.String:
                    if (SerialJoinPageMap.TryGetValue(join, out ushort serialPageID))
                    {
                        return serialPageID;
                    }

                    consoleLogger.Log($"[ReversePageRegistry] Warning: Could not find the page associated with the signal key of '{join}' with a type of '{type}'");

                    break;

                default:
                    return ushort.MaxValue;
            }

            throw new Exception($"[ReversePageRegistry] Error: Could not find the page associated with the signal key of '{join}' with a type of '{type}'");
        }

        /// <summary>
        /// Registers a join to a page within the registries.
        /// </summary>
        /// <param name="join">The join associated to use as a key.</param>
        /// <param name="pageId">The page to associate to the given key</param>
        /// <exception cref="InvalidOperationException">Gets thrown whenever a duplicate key is attempted to be used.</exception>
        public bool TryRegister(Enum join, ushort pageId)
        {
            eSigType type = enumHelper.GetSignalTypeFromEnum(join);
            uint joinNumber = Convert.ToUInt32(join);

            consoleLogger.Log($"[ReversePageRegistry] Log: Registering '{type}' join '{joinNumber}' to page '{pageId}'");

            switch (type)
            {
                case eSigType.Bool:
                    if (!DigitalJoinPageMap.ContainsKey(joinNumber))
                    {
                        DigitalJoinPageMap.Add(joinNumber, pageId);
                        return true;
                    } 
                    else
                    {
                        consoleLogger.Log($"[ReversePageRegistry] Error: Attempted to register a duplicate key of '{joinNumber}' for a type of '{type.ToString()}'");
                        return false;
                    }

                case eSigType.UShort:
                    if (!AnalogJoinPageMap.ContainsKey(joinNumber))
                    {
                        AnalogJoinPageMap.Add(joinNumber, pageId);
                        return true;
                    }
                    else
                    {
                        consoleLogger.Log($"[ReversePageRegistry] Error: Attempted to register a duplicate key of '{joinNumber}' for a type of '{type.ToString()}'");
                        return false;
                    }

                case eSigType.String:
                    if (!SerialJoinPageMap.ContainsKey(joinNumber))
                    {
                        SerialJoinPageMap.Add(joinNumber, pageId);
                        return true;
                    }
                    else
                    {
                        consoleLogger.Log($"[ReversePageRegistry] Error: Attempted to register a duplicate key of '{joinNumber}' for a type of '{type.ToString()}'");
                        return false;
                    }

                default:
                    throw new InvalidOperationException($"[ReversePageRegistry] Error: Unsupported eSigType of '{type}'");
            }
        }

        /// <summary>
        /// Clears all the entries in the registry. Use only at system shutdown.
        /// </summary>
        public void Dispose()
        {
            DigitalJoinPageMap.Clear();
            AnalogJoinPageMap.Clear();
            SerialJoinPageMap.Clear();
        }
    }
}
