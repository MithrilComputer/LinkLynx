
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
