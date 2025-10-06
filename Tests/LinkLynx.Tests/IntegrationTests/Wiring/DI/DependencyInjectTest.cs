using LinkLynx.Core.Interfaces.Utility.Debugging.Logging;
using LinkLynx.Wiring.DI;
using NUnit.Framework;
using LinkLynx.Implementations.Utility.Debugging.Logging;
using System;
using Crestron.SimplSharp;
using System.Text;

namespace LinkLynx.IntegrationTests.Wiring.DI
{
    [TestFixture]
    public class DependencyInjectTest
    {
        [Test]
        public void CreateDefault()
        {
            ServiceCollection services = new ServiceCollection();

            services.AddSingleton<ILogger, ConsoleLogger>();

            var d = services.GetCurrentDescriptorCollection()[0];

            Console.WriteLine($"ServiceType: {d.ServiceType} | {d.ServiceType.Assembly.FullName}");
            Console.WriteLine($"ImplType:    {d.ImplementationType} | {d.ImplementationType.Assembly.FullName}");
            Console.WriteLine($"Requested:   {typeof(ILogger)} | {typeof(ILogger).Assembly.FullName}");

            Assert.That(d.ServiceType, Is.EqualTo(typeof(ILogger)));                 // must pass
            Assert.That(typeof(ILogger).IsAssignableFrom(typeof(ConsoleLogger)));    // must pass

            ServiceProvider provider = services.BuildServiceProvider();

            var a = provider.GetRequired<ILogger>();
            var b = provider.GetRequired<ILogger>();

            Assert.AreSame(a, b);
        }
    }
}
