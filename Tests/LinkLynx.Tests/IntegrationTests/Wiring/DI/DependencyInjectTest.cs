using LinkLynx.Core.Interfaces;
using LinkLynx.Core.Utility.Debugging.Logging;
using LinkLynx.Wiring.DI;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkLynx.IntegrationTests.Wiring.DI
{
    [TestFixture]
    public class DependencyInjectTest
    {
        public static ServiceProvider CreateDefault()
        {
            ServiceCollection services = new ServiceCollection();

            services.AddSingleton<ILogger, ConsoleLogger>();

            ServiceProvider provider = services.BuildServiceProvider();

            var a = provider.GetRequired<ILogger>();
            var b = provider.GetRequired<ILogger>();

            Assert.AreSame(a, b);
        }
    }
}
