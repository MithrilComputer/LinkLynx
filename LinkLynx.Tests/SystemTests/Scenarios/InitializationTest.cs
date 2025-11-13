using LinkLynx.Core.CrestronPOCOs;
using LinkLynx.Core.Options;
using LinkLynx.Core.Signals;
using LinkLynx.PublicAPI.Implementations;
using LinkLynx.PublicAPI.Interfaces;
using LinkLynx.Tests.SystemTests.Mocks;
using Xunit;
using Xunit.Abstractions;

namespace LinkLynx.Tests.SystemTests.Scenarios
{
    public class InitializationTest(ITestOutputHelper console)
    {
        private readonly ITestOutputHelper console = console;

        private ILinkLynx? linkLynx;

        private System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();

        [Fact(DisplayName = "Builds a plain LinkLynx")]
        public void Initialization()
        {
            stopwatch.Start();

            try
            {
                LinkLynxBuildOptions buildOptions = new LinkLynxBuildOptions
                {
                    AutoRegisterPanelsToControlSystem = false
                };

                linkLynx = LinkLynxFactory.CreateLinkLynx(buildOptions);

                if (linkLynx == null)
                    throw new Exception("[InitializationTest] LinkLynx instance is null after creation.");

                console.WriteLine($"Time elapsed to start LinkLynx {stopwatch.Elapsed.TotalMilliseconds.ToString()} ms");

                stopwatch.Restart();

                linkLynx.RegisterPanel(ExternalTouchPanelMock.PanelOne);

                console.WriteLine($"Time elapsed to register panel {stopwatch.Elapsed.TotalMilliseconds.ToString()} ms");

                stopwatch.Restart();
            }
            catch (Exception ex)
            {
                console.WriteLine($"[InitializationTest] Exception during LinkLynx initialization: {ex.GetType().Name}: {ex.Message}");
                throw new Xunit.Sdk.XunitException("Test failed");
            }

            SignalEventData sig = new SignalEventData((int)TestButtons.Button1, SigType.Bool, true);
            linkLynx.HandleSimpleSignal(ExternalTouchPanelMock.PanelOne, sig);

            Assert.True(TestPage.NumberOfCalls == 1, "Expected TestPage.TestMethod to run when Button1 was routed.");

            linkLynx.SetPanelToDefaultState(ExternalTouchPanelMock.PanelOne);

            Assert.True(TestPage.NumberOfCalls == 0, "Expected TestPage.TestMethod to run when Button1 was routed.");

            console.WriteLine($"Time elapsed to handle signal and reset panel {stopwatch.Elapsed.TotalMilliseconds.ToString()} ms");

            stopwatch.Restart();

            linkLynx.Cleanup();

            console.WriteLine($"Time elapsed to cleanup LinkLynx {stopwatch.Elapsed.TotalMilliseconds.ToString()} ms");

            stopwatch.Stop();
        }
    }
}


