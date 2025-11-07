using LinkLynx.PublicAPI.Implementations;
using LinkLynx.PublicAPI.Interfaces;
using LinkLynx.Wiring.Bootstraps.Implementations;
using LinkLynx.Wiring.DI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkLynx.Tests.SystemTests.Fixtures
{
    public static class SystemTestAppFactory
    {
        public static ILinkLynx CreateDefaultImplementation()
        {
            TestBootStrap testBootStrap = new TestBootStrap();

            return LinkLynxFactory.CreateLinkLynxWithCustomDI(new Core.Options.LinkLynxBuildOptions(), testBootStrap);
        }
    }
}
