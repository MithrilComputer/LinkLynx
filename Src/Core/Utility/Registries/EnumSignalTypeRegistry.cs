using Crestron.SimplSharpPro;
using System;
using System.Collections.Generic;

namespace LinkLynx.Core.Utility.Registries
{
    /// <summary>
    /// A container class meant to hold a reference to what Enum type is associated with what eSigType it represents.
    /// </summary>
    internal sealed class EnumSignalTypeRegistry
    {
        private static readonly EnumSignalTypeRegistry instance = new EnumSignalTypeRegistry();

        /// <summary>
        /// The singleton instance of the class.
        /// </summary>
        public static EnumSignalTypeRegistry Instance => instance;

        /// <summary>
        /// Class constructor.
        /// </summary>
        internal EnumSignalTypeRegistry() { }

        /// <summary>
        /// The registry that holds the key value pairs.
        /// </summary>
        private readonly Dictionary<Type, eSigType> registry = new Dictionary<Type, eSigType>();

        /// <summary>
        /// How many items are in the registry.
        /// </summary>
        internal int Count => registry.Count;

        /// <summary>
        /// Gets a specific signal type from an enum type.
        /// </summary>
        /// <param name="enumType">A type representing an Enum</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="KeyNotFoundException"></exception>
        internal eSigType Get(Type enumType)
        {
            if(enumType == null)
                throw new ArgumentNullException($"[EnumSignalTypeRegistry] Error: Cant get a entry in the registry with a null Key!");

            if (!enumType.IsEnum) 
                throw new ArgumentException($"[EnumSignalTypeRegistry] Error: Type must be an enum. {nameof(enumType)}");

            if (registry.TryGetValue(enumType, out eSigType sigType))
            {
                return sigType;
            }

            throw new KeyNotFoundException($"[EnumSignalTypeRegistry] Error: Can't find Enum in registry: {enumType}");
        }

        /// <summary>
        /// Registers an Enum type to a signal type.
        /// </summary>
        /// <param name="enumType">The key Enum</param>
        /// <param name="type">The value eSigType</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        internal void Register(Type enumType, eSigType type)
        {
            if (enumType == null)
                throw new ArgumentNullException($"[EnumSignalTypeRegistry] Error: Can't add a null Enum in registry!");

            if (!enumType.IsEnum) 
                throw new ArgumentException($"[EnumSignalTypeRegistry] Error: Type must be an enum. {nameof(enumType)}");

            if (!registry.ContainsKey(enumType))
            {
                registry.Add(enumType, type);
                return;
            }

            throw new InvalidOperationException($"[EnumSignalTypeRegistry] Error: Can't add duplicate Enum to registry: {enumType}");
        }

        /// <summary>
        /// Clears the registry of all its entries.
        /// </summary>
        internal void Clear()
        {
            registry.Clear();
        }

        /// <summary>
        /// Checks if a type has already been registered.
        /// </summary>
        /// <param name="enumType">The Enum type to check.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        internal bool IsRegistered(Type enumType)
        {
            if (enumType == null)
                throw new ArgumentNullException($"[EnumSignalTypeRegistry] Error: Can't check a null Key!");

            if (!enumType.IsEnum)
                throw new ArgumentException($"[EnumSignalTypeRegistry] Error: Type must be an enum. {nameof(enumType)}");

            return registry.ContainsKey(enumType);
        }
    }
}
