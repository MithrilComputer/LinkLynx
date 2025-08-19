using Crestron.SimplSharpPro;
using LinkLynx.Core.Utility.Debugging.Logging;
using LinkLynx.Core.Utility.Dispatchers.Signals;
using LinkLynx.Core.Utility.Helpers;
using System;
using System.Collections.Generic;

namespace LinkLynx.Core.Utility.Registries
{
    /// <summary>
    /// A special class that used to link logic joins to pages, good for reverse join searching.
    /// </summary>
    internal sealed class ReversePageRegistry
    {
        /// <summary>
        /// The singleton instance of the class.
        /// </summary>
        private static readonly ReversePageRegistry instance = new ReversePageRegistry();

        /// <summary>
        /// The singleton instance of the class.
        /// </summary>
        internal static ReversePageRegistry Instance => instance;

        /// <summary>
        /// Class constructor.
        /// </summary>
        internal ReversePageRegistry() { }

        private readonly Dictionary<uint, ushort>  DigitalJoinPageMap = new Dictionary<uint, ushort>();
        private readonly Dictionary<uint, ushort> AnalogJoinPageMap = new Dictionary<uint, ushort>();
        private readonly Dictionary<uint, ushort> SerialJoinPageMap = new Dictionary<uint, ushort>();

        /// <summary>
        /// Gets a page ID given an input join ID and Join Type.
        /// </summary>
        /// <param name="join">The join ID to search for the page.</param>
        /// <param name="type">The type of join associated with the key.</param>
        /// <exception cref="Exception">This is thrown if a key could not be found.</exception>
        internal ushort GetPageFromSignalAndType(uint join, eSigType type)
        {
            switch (type)
            {
                case eSigType.Bool:
                    if (DigitalJoinPageMap.TryGetValue(join, out ushort digitalPageID))
                    {
                        return digitalPageID;
                    }
                    break;

                case eSigType.UShort:
                    if (AnalogJoinPageMap.TryGetValue(join, out ushort analogPageID))
                    {
                        return analogPageID;
                    }
                    break;

                case eSigType.String:
                    if (SerialJoinPageMap.TryGetValue(join, out ushort serialPageID))
                    {
                        return serialPageID;
                    }
                    break;
            }

            throw new Exception($"[GlobalJoinPageRegistry] Error: Could not find the page associated with the signal key of '{join}' with a type of '{type}'");
        }

        /// <summary>
        /// Registers a join to a page within the registries.
        /// </summary>
        /// <param name="join">The join associated to use as a key.</param>
        /// <param name="pageId">The page to associate to the given key</param>
        /// <exception cref="InvalidOperationException">Gets thrown whenever a duplicate key is attempted to be used.</exception>
        internal void RegisterPageKeyFromJoin(Enum join, ushort pageId)
        {
            eSigType type = EnumHelper.GetSignalTypeFromEnum(join);
            uint joinNumber = Convert.ToUInt32(join);

            ConsoleLogger.Log($"[GlobalJoinPageRegistry] Log: Registered '{type}' join '{joinNumber}' to page '{pageId}'");

            switch (type)
            {
                case eSigType.Bool:
                    if (!DigitalJoinPageMap.ContainsKey(joinNumber))
                    {
                        DigitalJoinPageMap.Add(joinNumber, pageId);
                    } 
                    else
                    {
                        throw new InvalidOperationException($"[GlobalJoinPageRegistry] Error: Duplicate digital key '{joinNumber}' was attempted to be registered to page '{pageId}'");
                    }
                        break;

                case eSigType.UShort:
                    if (!AnalogJoinPageMap.ContainsKey(joinNumber))
                    {
                        AnalogJoinPageMap.Add(joinNumber, pageId);
                    }
                    else
                    {
                        throw new InvalidOperationException($"[GlobalJoinPageRegistry] Error: Duplicate analog key '{joinNumber}' was attempted to be registered to page '{pageId}'");
                    }
                    break;

                case eSigType.String:
                    if (!SerialJoinPageMap.ContainsKey(joinNumber))
                    {
                        SerialJoinPageMap.Add(joinNumber, pageId);
                    }
                    else
                    {
                        throw new InvalidOperationException($"[GlobalJoinPageRegistry] Error: Duplicate serial key '{joinNumber}' was attempted to be registered to page '{pageId}'");
                    }
                    break;

                default:
                    throw new InvalidOperationException($"[GlobalJoinPageRegistry] Error: Unsupported eSigType of '{type}'");
            }
        }

        /// <summary>
        /// Clears all the entries in the registry. Use only at system shutdown.
        /// </summary>
        internal void Clear()
        {
            DigitalJoinPageMap.Clear();
            AnalogJoinPageMap.Clear();
            SerialJoinPageMap.Clear();
        }
    }
}
