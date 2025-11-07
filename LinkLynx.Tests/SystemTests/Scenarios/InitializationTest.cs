using Crestron.SimplSharpPro;
using LinkLynx.Core.Attributes;
using LinkLynx.Core.CrestronPOCOs;
using LinkLynx.Core.Logic.Pages;
using LinkLynx.Core.Signals;
using LinkLynx.PublicAPI.Implementations;
using LinkLynx.PublicAPI.Interfaces;
using LinkLynx.Tests.SystemTests.Fixtures;
using LinkLynx.Tests.SystemTests.Mocks;
using Xunit;
using Xunit.Abstractions;
using static LinkLynx.Tests.SystemTests.Scenarios.InitializationTest;

namespace LinkLynx.Tests.SystemTests.Scenarios
{
    public class InitializationTest(ITestOutputHelper console)
    {
        private readonly ITestOutputHelper console = console;

        private ILinkLynx linkLynx;

        [SigType(SigType.Bool)]
        public enum TestButtons
        {
            Button1 = 10
        }

        [Fact(DisplayName = "Builds a plain LinkLynx")]
        public void Initialization()
        {
            try
            {
                linkLynx = SystemTestAppFactory.CreateDefaultImplementation();

                if (linkLynx == null)
                    throw new Exception("[InitializationTest] LinkLynx instance is null after creation.");

                linkLynx.Initialize();

                linkLynx.RegisterPanel(ExternalTouchPanelMock.PanelOne);
            }
            catch (Exception ex)
            {
                console.WriteLine($"[InitializationTest] Exception during LinkLynx initialization: {ex.GetType().Name}: {ex.Message}");
                throw new Xunit.Sdk.XunitException("Test failed");
            }

            SignalEventData sig = new SignalEventData((int)TestButtons.Button1, SigType.Bool, true);
            linkLynx.HandleSimpleSignal(ExternalTouchPanelMock.PanelOne, sig);

            Assert.True(TestPage.WasFired, "Expected TestPage.TestMethod to run when Button1 was routed.");

            linkLynx.SetPanelToDefaultState(ExternalTouchPanelMock.PanelOne);

            Assert.False(TestPage.WasFired, "Expected TestPage.TestMethod to run when Button1 was routed.");
        }
    }

    [Page(1)]
    public class TestPage : PageLogicBase
    {
        public static bool WasFired { get; private set; }

        public TestPage(PanelDevice panel) : base(panel)
        {
        }

        public override void SetDefaults()
        {
           WasFired = false;
        }

        [Join(TestButtons.Button1)]
        public void TestMethod(SignalEventData args)
        {
            WasFired = true;
        }
    }
}


