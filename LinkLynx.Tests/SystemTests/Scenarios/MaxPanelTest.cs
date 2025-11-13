using LinkLynx.Core.CrestronPOCOs;
using LinkLynx.Core.Options;
using LinkLynx.Core.Signals;
using LinkLynx.PublicAPI.Implementations;
using LinkLynx.PublicAPI.Interfaces;
using LinkLynx.Tests.SystemTests.Fixtures;
using LinkLynx.Tests.SystemTests.Mocks;
using Xunit;
using Xunit.Abstractions;

namespace LinkLynx.Tests.SystemTests.Scenarios.MaxPanelTest
{
    public class MaxPanelTest(ITestOutputHelper console)
    {
        private readonly int NumberOfPanelsToTest = 252;

        private readonly ITestOutputHelper console = console;

        private ILinkLynx? linkLynx;

        private PanelDevice[]? panels;

        private readonly System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();

        [Fact(DisplayName = "Builds a highly loaded LinkLynx")]
        public void Initialization()
        {
            stopwatch.Start();

            try
            {
                LinkLynxBuildOptions options = new LinkLynxBuildOptions()
                {
                    AutoRegisterPanelsToControlSystem = false
                };

                linkLynx = LinkLynxFactory.CreateLinkLynx(options);

                if (linkLynx == null)
                    throw new Exception("[InitializationTest] LinkLynx instance is null after creation.");

                console.WriteLine($"Time elapsed to start LinkLynx {stopwatch.Elapsed.TotalMilliseconds.ToString()} ms");

                panels = ExternalTouchPanelMock.CreatePanelArray(NumberOfPanelsToTest);

                stopwatch.Restart();

                for (int i = 0; i < panels.Length; i++)
                {
                    linkLynx.RegisterPanel(panels[i]);
                }

                console.WriteLine($"Time elapsed to register {panels.Length} panels {stopwatch.Elapsed.TotalMilliseconds.ToString()} ms");

                stopwatch.Restart();

                for (int i = 0; i < panels.Length; i++)
                {
                    SignalEventData sig = new SignalEventData((int)TestButtons.Button1, SigType.Bool, true);
                    linkLynx.HandleSimpleSignal(panels[i], sig);

                    Assert.Equal(i + 1, TestPage.NumberOfCalls);
                }

                console.WriteLine($"Time elapsed to handle signals for {panels.Length} panels {stopwatch.Elapsed.TotalMilliseconds.ToString()} ms");

                stopwatch.Restart();

                linkLynx.SetPanelToDefaultState(panels[0]);

                linkLynx.Cleanup();

                console.WriteLine($"Time elapsed to cleanup LinkLynx {stopwatch.Elapsed.TotalMilliseconds.ToString()} ms");

                stopwatch.Stop();
            }
            catch (Exception ex)
            {
                console.WriteLine($"[InitializationTest] Exception during LinkLynx initialization: {ex.GetType().Name}: {ex.Message}  stack trace: '{ex.StackTrace}',");
                throw new Xunit.Sdk.XunitException("Test failed");
            }
        }
    }
}
