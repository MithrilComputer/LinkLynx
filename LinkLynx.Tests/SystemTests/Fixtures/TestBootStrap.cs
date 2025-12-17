using LinkLynx.Core.Interfaces.Collections.Dispatchers;
using LinkLynx.Core.Interfaces.Collections.Pools;
using LinkLynx.Core.Interfaces.Collections.Registries;
using LinkLynx.Core.Interfaces.Utility.Debugging.Logging;
using LinkLynx.Core.Interfaces.Utility.Dispatching;
using LinkLynx.Core.Interfaces.Utility.Factories;
using LinkLynx.Core.Interfaces.Utility.Helpers;
using LinkLynx.Core.Interfaces.Wiring.Engine;
using LinkLynx.Core.Options;
using LinkLynx.Core.Src.Core.Interfaces.Utility.Factories;
using LinkLynx.Core.Src.Implementations.Utility.Factories;
using LinkLynx.Implementations.Collections.Dispatchers.SimpleSignals;
using LinkLynx.Implementations.Collections.Pools;
using LinkLynx.Implementations.Collections.Registries;
using LinkLynx.Implementations.Utility.Dispatching;
using LinkLynx.Implementations.Utility.Factories;
using LinkLynx.Implementations.Utility.Helpers;
using LinkLynx.PublicAPI.Interfaces;
using LinkLynx.Tests.GeneralFixtures;
using LinkLynx.Wiring.Bootstraps.Interfaces;
using LinkLynx.Wiring.DI;
using LinkLynx.Wiring.Engine;

namespace LinkLynx.Tests.SystemTests.Fixtures
{
    internal class TestBootStrap : ILinkLynxBootstrap
    {
        public ServiceProvider CreateDefault(LinkLynxBuildOptions options)
        {
            ServiceCollection services = new ServiceCollection();

            // Logging
            services.AddSingleton<ILogger, TestLogger>();

            // Helpers
            services.AddSingleton<IEnumHelper, EnumHelper>();
            services.AddSingleton<ISignalHelper, SignalHelper>();
            services.AddSingleton<IJoinDispatcher, JoinDispatcher>();

            // Registries
            services.AddSingleton<IEnumSignalTypeRegistry, EnumSignalTypeRegistry>();
            services.AddSingleton<IPanelScriptRegistry, PanelScriptRegistry>();
            services.AddSingleton<ISimpleReversePanelScriptRegistry, SimpleReversePanelScriptRegistry>();

            // Pools
            services.AddSingleton<IPanelScriptGroupPool, PanelScriptGroupPool>();
            services.AddSingleton<IPanelPool, PanelPool>();

            // Dispatchers
            services.AddSingleton<IAnalogJoinDispatcher, AnalogDispatcher>();
            services.AddSingleton<IDigitalJoinDispatcher, DigitalDispatcher>();
            services.AddSingleton<ISerialJoinDispatcher, SerialDispatcher>();

            // Wiring / engine
            services.AddSingleton<IJoinDispatcher, JoinDispatcher>();
            services.AddSingleton<IAutoJoinRegistrar, AutoJoinRegistrar>();
            services.AddSingleton<IJoinInstanceRouter, JoinInstanceRouter>();
            services.AddSingleton<IAutoRegisterScanner, AutoRegisterScanner>();

            // Factories
            services.AddSingleton<IPageScriptFactory, PageScriptFactory>();
            services.AddSingleton<IPanelScriptGroupFactory, PanelScriptGroupFactory>();

            //LinkLynx
            services.AddSingleton(options);
            services.AddSingleton<ILinkLynx, PublicAPI.Implementations.LinkLynx>();

            return services.BuildServiceProvider();
        }
    }
}
