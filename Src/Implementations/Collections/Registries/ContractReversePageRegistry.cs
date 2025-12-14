using LinkLynx.Core.Interfaces.Collections.Registries;
using LinkLynx.Core.Interfaces.Utility.Debugging.Logging;

namespace LinkLynx.Implementations.Collections.Registries
{
    /// <summary>
    /// The Contract Reverse Page Registry is responsible for mapping contract joins to their respective page IDs.
    /// </summary>
    public sealed class ContractReversePageRegistry : IContractReversePageRegistry, IDisposable
    {
        private readonly ILogger logger;

        private readonly Dictionary<string, ushort> contractJoinPageMap = new Dictionary<string, ushort>();

        /// <summary>
        /// The constructor for the ContractReversePageRegistry.
        /// </summary>
        public ContractReversePageRegistry(ILogger logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Tries to register a contract join to a page ID.
        /// </summary>
        public bool TryRegister(string contractJoin, ushort pageId)
        {
            if (contractJoinPageMap.ContainsKey(contractJoin))
            {
                logger.Log($"[ContractReversePageRegistry] Warning: Contract Join '{contractJoin}' is already registered to another page. Skipping registration...");
                return false;
            }
            contractJoinPageMap[contractJoin] = pageId;
            return true;
        }

        /// <summary>
        /// Gets a page ID given an input contract join. Gives ushort.MaxValue if the key was not found.
        /// </summary>
        public bool TryGetPageId(string contractJoin, out ushort pageId)
        {
            if (contractJoinPageMap.TryGetValue(contractJoin, out ushort registeredPageId))
            {
                pageId = registeredPageId;
                return true;
            }

            pageId = ushort.MaxValue;

            logger.Log($"[ContractReversePageRegistry] Warning: Could not find the page associated with the contract join of '{contractJoin}'");

            return false;
        }

        /// <summary>
        /// Disposes the registry and clears all registered joins.
        /// </summary>
        public void Dispose()
        {
            contractJoinPageMap.Clear();
        }
    }
}
