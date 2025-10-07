using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.DeviceSupport;
using Crestron.SimplSharpPro.UI;
using LinkLynx.Core.Interfaces.Collections.Dispatchers;
using LinkLynx.Core.Interfaces.Collections.Pools;
using LinkLynx.Core.Interfaces.Collections.Registries;
using LinkLynx.Core.Interfaces.Utility.Debugging.Logging;
using LinkLynx.Core.Interfaces.Utility.Factories;
using LinkLynx.Core.Interfaces.Utility.Helpers;
using LinkLynx.Core.Logic.Pages;
using LinkLynx.Implementations.Collections.Dispatchers.SimpleSignals;
using LinkLynx.Implementations.Collections.Pools;
using LinkLynx.Implementations.Collections.Registries;
using LinkLynx.Implementations.Utility.Debugging.Logging;
using LinkLynx.Implementations.Utility.Factories;
using LinkLynx.Implementations.Utility.Helpers;
using LinkLynx.Wiring.DI;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace LinkLynx.Tests.UnitTests
{
    [TestFixture]
    internal class SystemTest
    {
        [Test]
        public void CreateDefault()
        {
            ServiceCollection services = new ServiceCollection();

            services.AddSingleton<ILogger, ConsoleLogger>();

            services.AddSingleton<IAnalogJoinDispatcher, AnalogDispatcher>();
            services.AddSingleton<ISerialJoinDispatcher, SerialDispatcher>();
            services.AddSingleton<IDigitalJoinDispatcher, DigitalDispatcher>();

            services.AddSingleton<IPageRegistry, PageRegistry>();
            services.AddSingleton<IPageFactory, PageFactory>();

            services.AddSingleton<ILogicGroupPool, LogicGroupPool>();

            services.AddSingleton<IEnumSignalTypeRegistry, EnumSignalTypeRegistry>();
            
            services.AddSingleton<IEnumHelper, EnumHelper>();
            services.AddSingleton<ISignalHelper, SignalHelper>();

            services.AddSingleton<IReversePageRegistry, ReversePageRegistry>();

            services.AddSingleton<>();
            services.AddSingleton<>();
            services.AddSingleton<>();

            ServiceProvider provider = services.BuildServiceProvider();

            var a = provider.GetRequired<ILogger>();
            var b = provider.GetRequired<ILogger>();

            Assert.AreSame(a, b);
        }


        
    }


    public delegate void VirtualSignalEvent(SigEventArgs args, BasicTriList panel);

    public class VirtualXPanel
    {
        private List<SigEventArgs> possibleSignals = new List<SigEventArgs>();

        private Random random;

        public event VirtualSignalEvent virtualSignalEvent;

        public XpanelForHtml5 Panel { get; private set; }

        public VirtualXPanel()
        {
            Panel = new XpanelForHtml5(0x03, new CrestronControlSystem());

            random = new Random();
        }

        public SigEventArgs TriggerSignalEvent()
        {
            int numberSelection = random.Next(possibleSignals.Count);

            SigEventArgs signal = possibleSignals[numberSelection];

            virtualSignalEvent.Invoke(signal, Panel);

            return signal;
        }

        private void GenerateSignals()
        {
            
        }
    }


    public class TestPage : PageLogicBase
    {
        public override void SetDefaults() { }

        public TestPage(BasicTriList device) : base(device) { }


    }
}
