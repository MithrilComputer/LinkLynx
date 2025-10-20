using LinkLynx.Wiring.DI;

namespace LinkLynx.Wiring.Bootstraps.Interfaces
{
    internal interface ILinkLynxBootstrap
    {
        ServiceProvider CreateDefault();
    }
}
