using LinkLynx.Wiring.DI;

namespace LinkLynx.Wiring.Bootstraps.Interfaces
{
    public interface ILinkLynxBootstrap
    {
        ServiceProvider CreateDefault();
    }
}
