using Independentsoft.Exchange;
using LinkLynx.Core.CrestronWrappers;
using LinkLynx.Core.Interfaces.Utility.Debugging.Logging;
using LinkLynx.Core.Options;
using LinkLynx.Core.Signals;
using LinkLynx.PublicAPI.Interfaces;
using LinkLynx.Tests.GeneralFixtures;
using LinkLynx.Tests.GeneralMocks;
using LinkLynx.Tests.SystemTests.Fixtures;
using LinkLynx.Tests.SystemTests.Mocks;
using NUnit.Framework;

namespace LinkLynx.Tests.SystemTests.Scenarios
{
    [TestFixture, Order(1)]
    public class WarmUpTest
    {
        private ILinkLynx? linkLynx;

        private readonly System.Diagnostics.Stopwatch stopwatch = new();

        private readonly ILogger logger = new TestLogger();

        [Test, Order(1), Category("Performance")]
        public void SinglePanelTest()
        {
            stopwatch.Start();

            LinkLynxBuildOptions options = new LinkLynxBuildOptions() { AutoRegisterPanelsToControlSystem = false };

            linkLynx = TestLinkLynxFactory.CreateLinkLynx(options);

            if (linkLynx == null)
                Assert.Fail("LinkLynx Was null");

            linkLynx?.Initialize();

            TestContext.WriteLine($"Time elapsed to start LinkLynx: {stopwatch.Elapsed.TotalMilliseconds} ms");

            stopwatch.Restart();

            TouchPanelDevice panel = new TouchPanelDevice(0x03, true, "TestPanel", logger);

            linkLynx?.RegisterPanel(panel);

            TestContext.WriteLine($"Time elapsed to register 1 panel: {stopwatch.Elapsed.TotalMilliseconds} ms");

            stopwatch.Restart();

            SignalEventData sig = new SignalEventData((int)TestButtons.Button1, SigType.Bool, true);
            linkLynx?.HandleSimpleSignal(panel, sig);

            Assert.That(TestPage.NumberOfCalls, Is.EqualTo(1));

            TestContext.WriteLine($"Time elapsed to handle signals for 1 panel: {stopwatch.Elapsed.TotalMilliseconds} ms");

            linkLynx?.SetPanelToDefaultState(panel);

            stopwatch.Restart();

            linkLynx?.Cleanup();

            TestContext.WriteLine($"Time elapsed to cleanup LinkLynx: {stopwatch.Elapsed.TotalMilliseconds} ms");
        }

        private const int MaxPanels = 252;

        [Test, Order(2), Category("Performance")]
        public void MaxPanelTest()
        {
            stopwatch.Start();

            LinkLynxBuildOptions options = new LinkLynxBuildOptions() { AutoRegisterPanelsToControlSystem = false };

            linkLynx = TestLinkLynxFactory.CreateLinkLynx(options);

            if (linkLynx == null)
                Assert.Fail("LinkLynx Was null");

            linkLynx?.Initialize();

            TestContext.WriteLine($"Time elapsed to start LinkLynx: {stopwatch.Elapsed.TotalMilliseconds} ms");

            TouchPanelDevice[] panels = TouchPanelMocks.CreatePanelArray(MaxPanels, logger);

            stopwatch.Restart();

            for (int i = 0; i < panels.Length; i++)
            {
                linkLynx?.RegisterPanel(panels[i]);
            }

            TestContext.WriteLine($"Time elapsed to register {panels.Length} panels: {stopwatch.Elapsed.TotalMilliseconds} ms");

            stopwatch.Restart();


            for (int i = 0; i < panels.Length; i++)
            {
                SignalEventData sig = new SignalEventData((int)TestButtons.Button1, SigType.Bool, true);

                linkLynx?.HandleSimpleSignal(panels[i], sig);

                Assert.That(TestPage.NumberOfCalls, Is.EqualTo(1 + i));
            }

            TestContext.WriteLine($"Time elapsed to handle signals for {panels.Length} panel: {stopwatch.Elapsed.TotalMilliseconds} ms");
            
            linkLynx?.SetPanelToDefaultState(panels[0]); // Sets the TestPage.NumberOfCalls to zero

            stopwatch.Restart();

            linkLynx?.Cleanup();

            TestContext.WriteLine($"Time elapsed to cleanup LinkLynx: {stopwatch.Elapsed.TotalMilliseconds} ms");

            stopwatch.Stop();
        }
    }
}
