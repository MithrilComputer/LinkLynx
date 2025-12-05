using LinkLynx.Core.Attributes;
using LinkLynx.Core.CrestronPOCOs;
using LinkLynx.Core.Logic.Pages;
using LinkLynx.Tests.GeneralMocks;

namespace LinkLynx.Tests.SystemTests.Mocks
{
    [Page(1)]
    public class TestPage : PageLogicScript
    {
        public static int NumberOfCalls { get; private set; }

        public TestPage(TouchPanelDevice panel) : base(panel)
        {
        }

        public override void SetDefaults()
        {
            NumberOfCalls = 0;
        }

        [Join(TestButtons.Button1)]
        public void TestMethod(SignalEventData args)
        {
            NumberOfCalls++;
        }
    }
}
