
using LinkLynx.Core.Interfaces.Utility.Debugging.Logging;
using LinkLynx.Implementations.Utility.Debugging.Logging;

namespace LinkLynx.Wiring.DI
{
    public static class LinkLynxBootstrap
    {
        public static ServiceProvider CreateDefault()
        {
            ServiceCollection services = new ServiceCollection();

            services.AddSingleton<ILogger, ConsoleLogger>();

            return services.BuildServiceProvider();
        }
    }
}
