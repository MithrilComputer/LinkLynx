using LinkLynx.Core.Interfaces.Collections.Registries;
using LinkLynx.Core.Interfaces.Utility.Debugging.Logging;
using LinkLynx.Core.Interfaces.Utility.Helpers;
using LinkLynx.Core.Signals;

namespace LinkLynx.Implementations.Collections.Registries
{
    /// <summary>
    /// A special class that used to link logic logic joins to pages, good for reverse join searching.
    /// </summary>
    public sealed class SimpleReversePanelScriptRegistry : ISimpleReversePanelScriptRegistry, IDisposable
    {
        private readonly ILogger consoleLogger;
        private readonly IEnumHelper enumHelper;

        /// <summary>
        /// Class constructor.
        /// </summary>
        public SimpleReversePanelScriptRegistry(ILogger consoleLogger, IEnumHelper enumHelper) 
        {
            this.consoleLogger = consoleLogger;
            this.enumHelper = enumHelper;
        }

        private readonly Dictionary<uint, ushort> digitalJoinPageMap = new Dictionary<uint, ushort>();
        private readonly Dictionary<uint, ushort> analogJoinPageMap = new Dictionary<uint, ushort>();
        private readonly Dictionary<uint, ushort> serialJoinPageMap = new Dictionary<uint, ushort>();

        /// <summary>
        /// Gets a page ID given an input join ID and Join Type. Gives ushort.MaxValue if the key was not found.
        /// </summary>
        /// <param name="join">The join ID to search for the page.</param>
        /// <param name="type">The type of join associated with the key.</param>
        /// <exception cref="ArgumentException"></exception>
        public bool TryGet(uint join, SigType type, out ushort pageID)
        {
            switch (type)
            {
                case SigType.Bool:
                    if (digitalJoinPageMap.TryGetValue(join, out ushort digitalPageID))
                    {
                        pageID = digitalPageID;
                        return true;
                    }

                    consoleLogger.Log($"[ReversePageRegistry] Warning: Could not find the page associated with the signal key of '{join}' with a type of '{type}'");

                    break;

                case SigType.UShort:
                    if (analogJoinPageMap.TryGetValue(join, out ushort analogPageID))
                    {
                        pageID = analogPageID;
                        return true;
                    }

                    consoleLogger.Log($"[ReversePageRegistry] Warning: Could not find the page associated with the signal key of '{join}' with a type of '{type}'");

                    break;

                case SigType.String:
                    if (serialJoinPageMap.TryGetValue(join, out ushort serialPageID))
                    {
                        pageID = serialPageID;
                        return true;
                    }

                    consoleLogger.Log($"[ReversePageRegistry] Warning: Could not find the page associated with the signal key of '{join}' with a type of '{type}'");

                    break;
            }

            pageID = ushort.MaxValue;

            return false;
        }

        /// <summary>
        /// Registers a join to a page within the registries.
        /// </summary>
        /// <param name="join">The join associated to use as a key.</param>
        /// <param name="pageId">The page to associate to the given key</param>
        /// <exception cref="InvalidOperationException">Gets thrown whenever a duplicate key is attempted to be used.</exception>
        public bool TryRegister(Enum join, ushort pageId)
        {
            SigType type = enumHelper.GetSignalTypeFromEnum(join);
            uint joinNumber = Convert.ToUInt32(join);

            consoleLogger.Log($"[ReversePageRegistry] Log: Registering '{type}' join '{joinNumber}' to page '{pageId}'");

            switch (type)
            {
                case SigType.Bool:
                    if (!digitalJoinPageMap.ContainsKey(joinNumber))
                    {
                        digitalJoinPageMap.Add(joinNumber, pageId);
                        return true;
                    } 
                    else
                    {
                        consoleLogger.Log($"[ReversePageRegistry] Error: Attempted to register a duplicate key of '{joinNumber}' for a type of '{type.ToString()}'");
                        return false;
                    }

                case SigType.UShort:
                    if (!analogJoinPageMap.ContainsKey(joinNumber))
                    {
                        analogJoinPageMap.Add(joinNumber, pageId);
                        return true;
                    }
                    else
                    {
                        consoleLogger.Log($"[ReversePageRegistry] Error: Attempted to register a duplicate key of '{joinNumber}' for a type of '{type.ToString()}'");
                        return false;
                    }

                case SigType.String:
                    if (!serialJoinPageMap.ContainsKey(joinNumber))
                    {
                        serialJoinPageMap.Add(joinNumber, pageId);
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
            digitalJoinPageMap.Clear();
            analogJoinPageMap.Clear();
            serialJoinPageMap.Clear();
        }
    }
}
