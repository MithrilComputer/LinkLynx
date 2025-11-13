using LinkLynx.Core.Options;
using LinkLynx.PublicAPI.Interfaces;
using LinkLynx.Wiring.Bootstraps.Implementations;
using LinkLynx.Wiring.DI;

namespace LinkLynx.PublicAPI.Implementations
{
    /// <summary>
    /// Provides factory methods for creating instances of the <see cref="ILinkLynx"/> interface.
    /// </summary>
    public static class LinkLynxFactory
    {
        /// <summary>
        /// Creates and initializes a default instance of the <see cref="ILinkLynx"/> interface.
        /// </summary>
        public static ILinkLynx CreateLinkLynx(LinkLynxBuildOptions options)
        {
            LinkLynxBootstrap bootstrapper = new LinkLynxBootstrap();

            ServiceProvider serviceProvider = bootstrapper.CreateDefault(options);

            return serviceProvider.GetRequired<ILinkLynx>();
        }
    }
}