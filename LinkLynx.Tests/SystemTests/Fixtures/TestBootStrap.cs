using LinkLynx.Core.Interfaces.Collections.Dispatchers;
using LinkLynx.Core.Interfaces.Collections.Pools;
using LinkLynx.Core.Interfaces.Collections.Registries;
using LinkLynx.Core.Interfaces.Utility.Debugging.Logging;
using LinkLynx.Core.Interfaces.Utility.Dispatching;
using LinkLynx.Core.Interfaces.Utility.Factories;
using LinkLynx.Core.Interfaces.Utility.Helpers;
using LinkLynx.Core.Interfaces.Wiring.Engine;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkLynx.Tests.SystemTests.Fixtures
{
    internal class TestBootStrap : ILinkLynxBootstrap
    {
        public ServiceProvider CreateDefault()
        {
            ServiceCollection services = new ServiceCollection();

            // Logging
            services.AddSingleton<ILogger, TestLogger>();

            // Helpers
            services.AddSingleton<IEnumHelper, EnumHelper>();
            services.AddSingleton<ISignalHelper, SignalHelper>();
            services.AddSingleton<JoinDispatcher, JoinDispatcher>();

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

            // Wiring / engine
            services.AddSingleton<IJoinDispatcher, JoinDispatcher>();
            services.AddSingleton<IAutoJoinRegistrar, AutoJoinRegistrar>();
            services.AddSingleton<IJoinInstanceRouter, JoinInstanceRouter>();
            services.AddSingleton<IAutoRegisterScanner, AutoRegisterScanner>();

            // Factories
            services.AddSingleton<IPageFactory, PageFactory>();

            return services.BuildServiceProvider();
        }
    }
}
