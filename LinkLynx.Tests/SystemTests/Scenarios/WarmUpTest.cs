using Independentsoft.Exchange;
using LinkLynx.Core.CrestronPOCOs;
using LinkLynx.Core.Options;
using LinkLynx.Core.Signals;
using LinkLynx.PublicAPI.Interfaces;
using LinkLynx.Tests.SystemTests.Fixtures;
using LinkLynx.Tests.SystemTests.Mocks;
using NUnit.Framework;

namespace LinkLynx.Tests.SystemTests.Scenarios
{
    [TestFixture]
    public class WarmUpTest
    {
        private ILinkLynx? linkLynx;

        private readonly System.Diagnostics.Stopwatch stopwatch = new();

        [Test, Order(1), Category("Performance")]
        public void WarmupTest()
        {
            stopwatch.Start();

            LinkLynxBuildOptions options = new LinkLynxBuildOptions() { AutoRegisterPanelsToControlSystem = false };

            linkLynx = TestLinkLynxFactory.CreateLinkLynx(options);

            if (linkLynx == null)
                Assert.Fail("LinkLynx Was null");

            linkLynx?.Initialize();

            TestContext.WriteLine($"Time elapsed to start LinkLynx: {stopwatch.Elapsed.TotalMilliseconds} ms");

            stopwatch.Restart();

            linkLynx?.RegisterPanel(ExternalTouchPanelMock.PanelOne);

            TestContext.WriteLine($"Time elapsed to register 1 panel: {stopwatch.Elapsed.TotalMilliseconds} ms");

            stopwatch.Restart();

            SignalEventData sig = new SignalEventData((int)TestButtons.Button1, SigType.Bool, true);
            linkLynx?.HandleSimpleSignal(ExternalTouchPanelMock.PanelOne, sig);

            Assert.That(TestPage.NumberOfCalls, Is.EqualTo(1));

            TestContext.WriteLine($"Time elapsed to handle signals for 1 panel: {stopwatch.Elapsed.TotalMilliseconds} ms");

            linkLynx?.SetPanelToDefaultState(ExternalTouchPanelMock.PanelOne);

            stopwatch.Restart();

            linkLynx?.Cleanup();
            TestContext.WriteLine($"Time elapsed to cleanup LinkLynx: {stopwatch.Elapsed.TotalMilliseconds} ms");
        }
    }
}
