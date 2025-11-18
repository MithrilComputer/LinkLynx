using LinkLynx.Core.CrestronPOCOs;
using LinkLynx.Core.Interfaces.Utility.Debugging.Logging;
using LinkLynx.Core.Options;
using LinkLynx.Core.Signals;
using LinkLynx.PublicAPI.Interfaces;
using LinkLynx.Tests.GeneralFixtures;
using LinkLynx.Tests.GeneralMocks;
using LinkLynx.Tests.SystemTests.Fixtures;
using LinkLynx.Wiring.DI;
using NUnit.Framework;

namespace LinkLynx.Tests.SystemTests.Scenarios
{
    [TestFixture, Order(2)]
    internal class PanelAddingTests
    {
        private ILinkLynx? linkLynx;

        private readonly ILogger logger = new TestLogger();

        [Test, Order(2), Category("Null Tests")]
        public void SingleNullPanelTest()
        {
            LinkLynxBuildOptions options = new LinkLynxBuildOptions() { AutoRegisterPanelsToControlSystem = false };

            linkLynx = TestLinkLynxFactory.CreateLinkLynx(options);

            if (linkLynx == null)
                Assert.Fail("LinkLynx Was null");

            linkLynx?.Initialize();

            PanelDevice? nullPanel = null;

            Assert.Throws<ArgumentNullException>(() => linkLynx?.RegisterPanel(nullPanel));

            SignalEventData sig = new SignalEventData((int)TestButtons.Button1, SigType.Bool, true);

            Assert.Throws<ArgumentNullException>(() => linkLynx?.HandleSimpleSignal(nullPanel, sig));

            linkLynx?.Cleanup();
        }

        [Test, Order(2), Category("Null Tests")]
        public void DuplicatePanelRegisterTest()
        {
            LinkLynxBuildOptions options = new LinkLynxBuildOptions() { AutoRegisterPanelsToControlSystem = false };

            TestBootStrap bootstrapper = new TestBootStrap();

            ServiceProvider serviceProvider = bootstrapper.CreateDefault(options);

            linkLynx = serviceProvider.GetRequired<ILinkLynx>();

            ILogger logger = serviceProvider.GetRequired<ILogger>();

            if (linkLynx == null)
                Assert.Fail("LinkLynx Was null");

            linkLynx?.Initialize();

            PanelDevice panel = new PanelDevice(0x03, true, "DuplicateTestPanel", logger);

            linkLynx?.RegisterPanel(panel);

            linkLynx?.RegisterPanel(panel);

            Assert.That(logger.Messages.Last(), Does.Contain("already registered"));

            linkLynx?.Cleanup();
        }
    }
}
