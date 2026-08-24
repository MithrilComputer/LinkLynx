using Crestron.SimplSharpPro;
using LinkLynx.Core.Abstractions.Debugging.Logging;
using LinkLynx.Core.Debugging.Logging;
using LinkLynx.Platform.Crestron.Abstractions.Wiring;
using Microsoft.Extensions.DependencyInjection;

namespace LinkLynx.Platform.Crestron
{
    internal class ControlSystem : CrestronControlSystem
    {
        public ControlSystem() : base()
        {
            
        }

        public override void InitializeSystem()
        {
            IBootstrap bootstrap = new LinkLynxBootstrap();

            ServiceProvider provider = bootstrap.CreateDefault();

            provider.GetRequiredService<ILogger>().Log(LogLevel.Info, "Test pass!");
            provider.GetRequiredService<IContextLogger<ControlSystem>>().Log(LogLevel.Info, "Test pass!");
        }
    }
}
