using LinkLynx.Core.Options;
using LinkLynx.PublicAPI.Interfaces;
using LinkLynx.Wiring.Bootstraps.Implementations;
using LinkLynx.Wiring.DI;

namespace LinkLynx.Tests.SystemTests.Fixtures
{
    public static class TestLinkLynxFactory
    {
        public static ILinkLynx CreateLinkLynx(LinkLynxBuildOptions options)
        {
            TestBootStrap bootstrapper = new TestBootStrap();

            ServiceProvider serviceProvider = bootstrapper.CreateDefault(options);

            return serviceProvider.GetRequired<ILinkLynx>();
        }
    }
}
