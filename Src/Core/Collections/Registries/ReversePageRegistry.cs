using Crestron.SimplSharpPro;
using LinkLynx.Core.Interfaces;
using LinkLynx.Core.Utility.Debugging.Logging;
using LinkLynx.Core.Utility.Helpers;
using System;
using System.Collections.Generic;

namespace LinkLynx.Core.Utility.Registries
{
    /// <summary>
    /// A special class that used to link logic joins to pages, good for reverse join searching.
    /// </summary>
    internal sealed class ReversePageRegistry : IReversePageRegistry
    {
        /// <summary>
        /// Creates and returns a new instance of a reverse page registry.
        /// </summary>
        public IReversePageRegistry Create() { return new ReversePageRegistry(); }

        /// <summary>
        /// Class constructor.
        /// </summary>
        private ReversePageRegistry() { }

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
            eSigType type = EnumHelper.GetSignalTypeFromEnum(join);
            uint joinNumber = Convert.ToUInt32(join);

            ConsoleLogger.Log($"[ReversePageRegistry] Log: Registered '{type}' join '{joinNumber}' to page '{pageId}'");

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
                        return false;
                        //throw new InvalidOperationException($"[ReversePageRegistry] Error: Duplicate analog key '{joinNumber}' was attempted to be registered to page '{pageId}'");
                    }

                case eSigType.String:
                    if (!SerialJoinPageMap.ContainsKey(joinNumber))
                    {
                        SerialJoinPageMap.Add(joinNumber, pageId);
                        return true;
                    }
                    else
                    {
                        return false;
                        //throw new InvalidOperationException($"[ReversePageRegistry] Error: Duplicate serial key '{joinNumber}' was attempted to be registered to page '{pageId}'");
                    }

                default:
                    throw new InvalidOperationException($"[ReversePageRegistry] Error: Unsupported eSigType of '{type}'");
            }
        }

        /// <summary>
        /// Clears all the entries in the registry. Use only at system shutdown.
        /// </summary>
        public void Clear()
        {
            DigitalJoinPageMap.Clear();
            AnalogJoinPageMap.Clear();
            SerialJoinPageMap.Clear();
        }
    }
}
