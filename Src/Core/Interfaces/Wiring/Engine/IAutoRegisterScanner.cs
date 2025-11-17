using LinkLynx.Core.Attributes;

namespace LinkLynx.Core.Interfaces.Wiring.Engine
{
    /// <summary>
    /// Provides methods for automatically scanning and registering pages and their associated signals
    /// within the system.
    /// </summary>
    public interface IAutoRegisterScanner
    {
        /// <summary>
        /// Executes the scanner to find and register all page logic types and any associated signal joins
        /// across all assemblies in the current application domain.
        /// </summary>
        void Run();

        /// <summary>
        /// Registers join signals of a given page type to the global registry using the AutoJoinRegistrar.
        /// </summary>
        /// <param name="pageType">The specific page class type to wire joins for.</param>
        /// <param name="pageId">The unique ID associated with the page.</param>
        void AutoWireJoins(Type pageType, ushort pageId);

        /// <summary>
        /// Attempts to register an enum type marked with a <see cref="Core.Attributes.SigTypeAttribute"/>
        /// to the <see cref="Core.Interfaces.Collections.Registries.IEnumSignalTypeRegistry"/>.
        /// </summary>
        /// <param name="type">The enum type to register. Must be decorated with <see cref="SigTypeAttribute"/>.</param>
        void TryRegisterEnumSigType(Type type);
    }
}
