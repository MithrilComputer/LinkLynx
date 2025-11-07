using LinkLynx.Core.Interfaces.Collections.Pools;
using LinkLynx.Core.Interfaces.Utility.Debugging.Logging;
using LinkLynx.Core.Interfaces.Utility.Dispatching;
using LinkLynx.Core.Options;
using LinkLynx.Core.Interfaces.Wiring.Engine;
using LinkLynx.PublicAPI.Interfaces;
using LinkLynx.Wiring.Bootstraps.Implementations;
using LinkLynx.Wiring.DI;
using System;
using System.Runtime.CompilerServices;
using LinkLynx.Wiring.Bootstraps.Interfaces;

[assembly: InternalsVisibleTo("LinkLynx.Tests")]
namespace LinkLynx.PublicAPI.Implementations
{
    /// <summary>
    /// Provides factory methods for creating instances of the <see cref="ILinkLynx"/> interface.
    /// </summary>
    public static class LinkLynxFactory
    {
        // TODO fix this shit later, its not a good solution but I want to get this done.
        // Modify the DI to auto register its self so that its as easy as just passing in the build options to it.
        // Not to mention get rid of the bool check

        private static bool createdInstance = false;

        /// <summary>
        /// Creates and initializes a default instance of the <see cref="ILinkLynx"/> interface.
        /// </summary>
        public static ILinkLynx CreateLinkLynx(LinkLynxBuildOptions options)
        {
            if(!createdInstance)
            {
                LinkLynxBootstrap bootstrapper = new LinkLynxBootstrap();

                ServiceProvider serviceProvider = bootstrapper.CreateDefault();

                createdInstance = true;

                return new LinkLynx(serviceProvider,
                    serviceProvider.GetRequired<ILogger>(),
                    serviceProvider.GetRequired<IAutoRegisterScanner>(),
                    serviceProvider.GetRequired<ILogicGroupPool>(),
                    serviceProvider.GetRequired<IJoinInstanceRouter>(),
                    options.AutoRegisterPanelsToControlSystem,
                    serviceProvider.GetRequired<IPanelPool>());
            }

            throw new InvalidOperationException("LinkLynx instance has already been created. Multiple instances are not supported.");
        }

        /// <summary>
        /// Creates and initializes a default instance of the <see cref="ILinkLynx"/> interface.
        /// </summary>
        internal static ILinkLynx CreateLinkLynxWithCustomDI(LinkLynxBuildOptions options, ILinkLynxBootstrap bootstrap)
        {
            if (!createdInstance)
            {
                ServiceProvider serviceProvider = bootstrap.CreateDefault();

                createdInstance = true;

                return new LinkLynx(serviceProvider,
                    serviceProvider.GetRequired<ILogger>(),
                    serviceProvider.GetRequired<IAutoRegisterScanner>(),
                    serviceProvider.GetRequired<ILogicGroupPool>(),
                    serviceProvider.GetRequired<IJoinInstanceRouter>(),
                    options.AutoRegisterPanelsToControlSystem,
                    serviceProvider.GetRequired<IPanelPool>());
            }

            throw new InvalidOperationException("LinkLynx instance has already been created. Multiple instances are not supported.");
        }
    }
}
