namespace LinkLynx.Core.Src.Core.Interfaces.Collections.Registries
{
    /// <summary>
    /// The Contract Name Registry is responsible for keeping track of registered contract names.
    /// </summary>
    public interface IContractNameRegistry
    {
        /// <summary>
        /// Tries to register a contract name.
        /// </summary>
        bool TryRegister(string contractName);

        /// <summary>
        /// Checks if a contract name is in the Registry.
        /// </summary>
        bool IsRegistered(string contractName);
    }
}
