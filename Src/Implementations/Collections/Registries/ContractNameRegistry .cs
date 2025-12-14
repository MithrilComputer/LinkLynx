using LinkLynx.Core.Interfaces.Utility.Debugging.Logging;
using LinkLynx.Core.Src.Core.Interfaces.Collections.Registries;

namespace LinkLynx.Implementations.Collections.Registries
{
    /// <summary>
    /// The Contract Name Registry is responsible for keeping track of registered contract names.
    /// </summary>
    public sealed class ContractNameRegistry : IContractNameRegistry, IDisposable
    {
        private readonly List<string> registeredContractNames = new List<string>();

        private readonly ILogger logger;

        /// <summary>
        /// The constructor for the ContractNameRegistry.
        /// </summary>
        public ContractNameRegistry(ILogger logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Tries to register a contract name.
        /// </summary>
        public bool TryRegister(string contractName)
        {
            if (registeredContractNames.Contains(contractName))
            {
                logger.Log($"[ContractNameRegistry] Warning: Contract Name '{contractName}' is already registered. Skipping registration...");
                return false;
            }
            registeredContractNames.Add(contractName);
            return true;
        }

        /// <summary>
        /// Checks if a contract name is in the Registry.
        /// </summary>
        public bool IsRegistered(string contractName)
        {
            return registeredContractNames.Contains(contractName);
        }

        /// <summary>
        /// Disposes the registry and clears all registered contract names.
        /// </summary>
        public void Dispose()
        {
            registeredContractNames.Clear();
        }
    }
}
