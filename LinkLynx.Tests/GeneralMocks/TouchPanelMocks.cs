using LinkLynx.Core.CrestronPOCOs;
using LinkLynx.Core.Interfaces.Utility.Debugging.Logging;

namespace LinkLynx.Tests.GeneralMocks
{
    public static class TouchPanelMocks
    {
        public static TouchPanelDevice[] CreatePanelArray(int numberOfPanels, ILogger logger)
        {
            TouchPanelDevice[] panels = new TouchPanelDevice[numberOfPanels];

            for (int i = 0; i < numberOfPanels; i++)
            {
                panels[i] = new TouchPanelDevice((uint)i + 3, true, $"Panel {i}", logger);
            }

            return panels;
        }
    }
}
