using Microsoft.Extensions.DependencyInjection;

namespace LinkLynx.Platform.Crestron.Abstractions.Wiring
{
    internal interface IBootstrap
    {
        ServiceProvider CreateDefault();
    }
}
