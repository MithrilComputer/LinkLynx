using LinkLynx.Core.Interfaces.Utility.Debugging.Logging;
using LinkLynx.Core.Signals;
using LinkLynx.Core.Src.Core.Interfaces.Collections.Registries;

namespace LinkLynx.Implementations.Collections.Registries
{
    /// <summary>
    /// The Contract Name Registry is responsible for keeping track of registered contract names.
    /// </summary>
    public sealed class ContractIDNameRegistry : IContractIDNameRegistry, IDisposable
    {
        /// <summary>
        /// A dictionary to hold incoming contract names and types.
        /// </summary>
        private readonly Dictionary<(uint, SigType), string> incomingIDContractNames = new();

        /// <summary>
        /// A dictionary to hold outgoing contract names and types.
        /// </summary>
        private readonly Dictionary<(uint, SigType), string> outgoingIDContractNames = new();

        /// <summary>
        /// A dictionary to hold incoming contract names and types.
        /// </summary>
        private readonly Dictionary<string, (uint, SigType)> incomingContractJoinIDs= new();

        /// <summary>
        /// A dictionary to hold outgoing contract names and types.
        /// </summary>
        private readonly Dictionary<string, (uint, SigType)> outgoingContractJoinIDs = new();

        /// <summary>
        /// A logger for logging registry actions.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// The constructor for the ContractNameRegistry.
        /// </summary>
        public ContractIDNameRegistry(ILogger logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Tries to register a contract name with a join number and signal direction.
        /// </summary>
        public bool TryRegister(uint joinNumber, SigType sigType,  string contractName, SignalDirection direction)
        {
            if (string.IsNullOrWhiteSpace(contractName))
            {
                logger.Log("Contract name cannot be null or whitespace.");
                return false;
            }

            if (direction == SignalDirection.Incoming)
            {
                if (incomingIDContractNames.ContainsKey((joinNumber, sigType)))
                {
                    logger.Log($"Contract name '{contractName}' is already registered '{joinNumber}'.");
                    return false;
                }

                incomingIDContractNames.Add((joinNumber, sigType), contractName);

                incomingContractJoinIDs.Add(contractName, (joinNumber, sigType));

                logger.Log($"Registered contract name '{contractName}' with join number '{joinNumber}'.");

                return true;
            }
            else if (direction == SignalDirection.Outgoing)
            {
                if (outgoingIDContractNames.ContainsKey((joinNumber, sigType)))
                {
                    logger.Log($"Contract name '{contractName}' is already registered '{joinNumber}'.");
                    return false;
                }

                outgoingIDContractNames.Add((joinNumber, sigType), contractName);

                outgoingContractJoinIDs.Add(contractName, (joinNumber, sigType));

                logger.Log($"Registered contract name '{contractName}' with join number '{joinNumber}'.");

                return true;
            }
            else
            {
                logger.Log($"Invalid signal direction '{direction}' for contract name '{contractName}'.");
                return false;
            }
        }


        /// <summary>
        /// Gets a contract name by join number and signal direction.
        /// </summary>
        public bool TryGetFromIDAndSigType(uint joinNumber, SigType sigType, SignalDirection direction, out string name)
        {
            if(direction == SignalDirection.Incoming)
            {
                return incomingIDContractNames.TryGetValue((joinNumber, sigType), out name);
            }
            else if(direction == SignalDirection.Outgoing)
            {
                return outgoingIDContractNames.TryGetValue((joinNumber, sigType), out name);
            }
            else
            {
                name = string.Empty;
                return false;
            }
        }

        /// <summary>
        /// Gets a join number and signal type by contract name and signal direction.
        /// </summary>
        public bool TryGetFromContractName(string contractName, SignalDirection direction, out (uint, SigType) joinIDAndSigType)
        {
            if (direction == SignalDirection.Incoming)
            {
                return incomingContractJoinIDs.TryGetValue(contractName, out joinIDAndSigType);
            }
            else if (direction == SignalDirection.Outgoing)
            {
                return outgoingContractJoinIDs.TryGetValue(contractName, out joinIDAndSigType);
            }
            else
            {
                joinIDAndSigType = new(); //Empty tuple

                return false;
            }
        }

        /// <summary>
        /// Checks if a contract name is registered for a given join number and signal direction.
        /// </summary>
        public bool IsRegistered(uint joinNumber, SigType sigType, SignalDirection direction)
        {
            if(direction == SignalDirection.Incoming)
            {
                return incomingIDContractNames.ContainsKey((joinNumber, sigType));
            }
            else if(direction == SignalDirection.Outgoing)
            {
                return outgoingIDContractNames.ContainsKey((joinNumber, sigType));
            }

            return false;
        }

        /// <summary>
        /// Checks if a contract name is registered for a given signal direction.
        /// </summary>
        public bool IsRegistered(string contractName, SignalDirection direction)
        {
            if (direction == SignalDirection.Incoming)
            {
                return incomingContractJoinIDs.ContainsKey(contractName);
            }
            else if (direction == SignalDirection.Outgoing)
            {
                return outgoingContractJoinIDs.ContainsKey(contractName);
            }

            return false;
        }

        /// <summary>
        /// Disposes the registry and clears all registered contract names.
        /// </summary>
        public void Dispose()
        {
            incomingIDContractNames.Clear();
            outgoingIDContractNames.Clear();
            incomingContractJoinIDs.Clear();
            outgoingContractJoinIDs.Clear();
        }
    }
}
