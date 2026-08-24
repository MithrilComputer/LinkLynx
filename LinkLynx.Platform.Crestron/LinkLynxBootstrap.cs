using LinkLynx.Core.Abstractions.Debugging.Logging;
using LinkLynx.Platform.Crestron.Abstractions.Wiring;
using LinkLynx.Platform.Crestron.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace LinkLynx.Platform.Crestron
{
    internal class LinkLynxBootstrap : IBootstrap
    {
        public ServiceProvider CreateDefault()
        {
            ServiceCollection serviceCollection = new ServiceCollection();

            // Logging
            serviceCollection.AddSingleton<ILogger, CrestronLogger>();
            serviceCollection.AddTransient(typeof(IContextLogger<>), typeof(CrestronContextLogger<>));

            //serviceCollection.AddSingleton<ControlSystem>(); TODO make a common control system interface


            return serviceCollection.
                BuildServiceProvider(
                new ServiceProviderOptions {
                    ValidateOnBuild = true,
                    ValidateScopes = true
                });
        }
    }
}
