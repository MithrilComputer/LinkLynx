using LinkLynx.Core.CrestronPOCOs;
using LinkLynx.Core.Interfaces.Utility.Debugging.Logging;

namespace LinkLynx.Tests.GeneralMocks
{
    public static class TouchPanelMocks
    {
        public static PanelDevice[] CreatePanelArray(int numberOfPanels, ILogger logger)
        {
            PanelDevice[] panels = new PanelDevice[numberOfPanels];

            for (int i = 0; i < numberOfPanels; i++)
            {
                panels[i] = new PanelDevice((uint)i + 3, true, $"Panel {i}", logger);
            }

            return panels;
        }
    }
}
