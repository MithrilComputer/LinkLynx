using LinkLynx.Core.Options;
using LinkLynx.Wiring.DI;

namespace LinkLynx.Wiring.Bootstraps.Interfaces
{
    /// <summary>
    /// The LinkLynx bootstrap interface defines a contract for creating a default service provider for dependency injection.
    /// </summary>
    public interface ILinkLynxBootstrap
    {
        /// <summary>
        /// Creates a default service provider for dependency injection.
        /// </summary>
        ServiceProvider CreateDefault(LinkLynxBuildOptions options);
    }
}
