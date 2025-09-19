using Crestron.SimplSharpPro;
using System;

namespace LinkLynx.Core.Interfaces
{
    /// <summary>
    /// An Interface designed to hold all enums that should have a Signal Type, eg, Digital, Serial, Analog.
    /// </summary>
    public interface IEnumSignalTypeRegistry
    {
        /// <summary>
        /// Checks if the enum is registered.
        /// </summary>
        bool IsRegistered(Type enumType);

        /// <summary>
        /// Clears the registry entries.
        /// </summary>
        void Clear();

        /// <summary>
        /// The amount of entries in the registry.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Registers an enum to the signal type.
        /// </summary>
        void Register(Type enumType, eSigType type);

        /// <summary>
        /// Gets the signal type associated with the given enum.
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        eSigType Get(Type enumType);
    }
}
