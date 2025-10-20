using LinkLynx.Core.Interfaces.Collections.Dispatchers;
using LinkLynx.Core.Interfaces.Collections.Pools;
using LinkLynx.Core.Interfaces.Collections.Registries;
using LinkLynx.Core.Interfaces.Utility.Debugging.Logging;
using LinkLynx.Core.Interfaces.Utility.Dispatching;
using LinkLynx.Core.Interfaces.Utility.Factories;
using LinkLynx.Core.Interfaces.Utility.Helpers;
using LinkLynx.Core.Src.Core.Interfaces.Wiring.Engine;
using LinkLynx.Implementations.Collections.Dispatchers.SimpleSignals;
using LinkLynx.Implementations.Collections.Pools;
using LinkLynx.Implementations.Collections.Registries;
using LinkLynx.Implementations.Utility.Debugging.Logging;
using LinkLynx.Implementations.Utility.Dispatching;
using LinkLynx.Implementations.Utility.Factories;
using LinkLynx.Implementations.Utility.Helpers;
using LinkLynx.Wiring.Bootstraps.Interfaces;
using LinkLynx.Wiring.DI;
using LinkLynx.Wiring.Engine;

namespace LinkLynx.Wiring.Bootstraps.Implementations
{
    /// <summary>
    /// Provides methods to initialize and configure a default <see cref="ServiceProvider"/> instance  with a predefined
    /// set of services.
    /// </summary>
    /// <remarks>The <see cref="LinkLynxBootstrap"/> class is responsible for setting up a dependency
    /// injection  container with commonly used services, such as logging, signal dispatchers, page management,  and
    /// utility helpers. This class simplifies the process of creating a fully configured  <see cref="ServiceProvider"/>
    /// for use in applications.</remarks>
    public class LinkLynxBootstrap : ILinkLynxBootstrap
    {
        /// <summary>
        /// Creates and configures a default <see cref="ServiceProvider"/> instance with predefined services.
        /// </summary>
        public ServiceProvider CreateDefault()
        {
            ServiceCollection services = new ServiceCollection();

            // Logging
            services.AddSingleton<ILogger, ConsoleLogger>();

            // Helpers
            services.AddSingleton<IEnumHelper, EnumHelper>();
            services.AddSingleton<ISignalHelper, SignalHelper>();
            services.AddSingleton<DispatcherHelper, DispatcherHelper>();

            // Registries
            services.AddSingleton<IEnumSignalTypeRegistry, EnumSignalTypeRegistry>();
            services.AddSingleton<IPageRegistry, PageRegistry>();
            services.AddSingleton<IReversePageRegistry, ReversePageRegistry>();

            // Pools
            services.AddSingleton<ILogicGroupPool, LogicGroupPool>();
            services.AddSingleton<IPanelPool, PanelPool>();

            // Dispatchers
            services.AddSingleton<IAnalogJoinDispatcher, AnalogDispatcher>();
            services.AddSingleton<IDigitalJoinDispatcher, DigitalDispatcher>();
            services.AddSingleton<ISerialJoinDispatcher, SerialDispatcher>();
            // services.AddSingleton<IJoinDispatcher, CompositeJoinDispatcher>(); // if you have one

            // Wiring / engine
            services.AddSingleton<IAutoJoinRegistrar, AutoJoinRegistrar>();
            services.AddSingleton<IJoinInstanceRouter, JoinInstanceRouter>();
            services.AddSingleton<IAutoRegisterScanner, AutoRegisterScanner>();

            // Factories
            services.AddSingleton<IPageFactory, PageFactory>();

            return services.BuildServiceProvider();
        }
    }
}
