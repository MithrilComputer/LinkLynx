using LinkLynx.Core.Interfaces.Collections.Dispatchers;
using LinkLynx.Core.Interfaces.Collections.Pools;
using LinkLynx.Core.Interfaces.Collections.Registries;
using LinkLynx.Core.Interfaces.Utility.Debugging.Logging;
using LinkLynx.Core.Interfaces.Utility.Factories;
using LinkLynx.Core.Interfaces.Utility.Helpers;
using LinkLynx.Implementations.Collections.Dispatchers.SimpleSignals;
using LinkLynx.Implementations.Collections.Pools;
using LinkLynx.Implementations.Collections.Registries;
using LinkLynx.Implementations.Utility.Debugging.Logging;
using LinkLynx.Implementations.Utility.Factories;
using LinkLynx.Implementations.Utility.Helpers;
using LinkLynx.Wiring.Bootstraps.Interfaces;
using LinkLynx.Wiring.DI;

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

            services.AddSingleton<ILogger, ConsoleLogger>();

            services.AddSingleton<IPanelPool, PanelPool>();

            services.AddSingleton<IAnalogJoinDispatcher, AnalogDispatcher>();
            services.AddSingleton<ISerialJoinDispatcher, SerialDispatcher>();
            services.AddSingleton<IDigitalJoinDispatcher, DigitalDispatcher>();

            services.AddSingleton<IPageRegistry, PageRegistry>();
            services.AddSingleton<IPageFactory, PageFactory>();

            services.AddSingleton<ILogicGroupPool, LogicGroupPool>();

            services.AddSingleton<IEnumSignalTypeRegistry, EnumSignalTypeRegistry>();

            services.AddSingleton<IEnumHelper, EnumHelper>();
            services.AddSingleton<ISignalHelper, SignalHelper>();

            services.AddSingleton<IReversePageRegistry, ReversePageRegistry>();

            return services.BuildServiceProvider();
        }
    }
}
