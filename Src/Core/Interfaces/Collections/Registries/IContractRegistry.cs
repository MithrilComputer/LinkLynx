using LinkLynx.Core.Signals;

namespace LinkLynx.Core.Interfaces.Collections.Registries
{
    /// <summary>
    /// The Contract Name Registry is responsible for keeping track of registered contract names.
    /// </summary>
    public interface IContractRegistry
    {
        /// <summary>
        /// Tries to register a contract name with a join number and signal direction.
        /// </summary>
        bool TryRegister(uint joinNumber, SigType sigType, string contractName, SignalDirection direction);

        /// <summary>
        /// Gets a contract name by join number and signal direction.
        /// </summary>
        bool TryGetFromIDAndSigType(uint joinNumber, SigType sigType, SignalDirection direction, out string name);

        /// <summary>
        /// Gets a join number and signal type by contract name and signal direction.
        /// </summary>
        bool TryGetFromContractName(string contractName, SignalDirection direction, out (uint, SigType) joinIDAndSigType);

        /// <summary>
        /// Checks if a contract name is registered for a given join number and signal direction.
        /// </summary>
        bool IsRegistered(uint joinNumber, SigType sigType, SignalDirection direction);

        /// <summary>
        /// Checks if a contract name is registered for a given signal direction.
        /// </summary>
        bool IsRegistered(string contractName, SignalDirection direction);
    }
}
