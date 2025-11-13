using LinkLynx.Core.CrestronPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkLynx.Tests.SystemTests.Mocks
{
    public static class ExternalTouchPanelMock
    {
        public static PanelDevice PanelOne { get; private set; }
        public static PanelDevice PanelTwo { get; private set; }
        public static PanelDevice PanelThree { get; private set; }
        public static PanelDevice PanelFour { get; private set; }
        public static PanelDevice PanelFive { get; private set; }
        public static PanelDevice PanelSix { get; private set; }
        public static PanelDevice PanelSeven { get; private set; }
        public static PanelDevice PanelEight { get; private set; }
        public static PanelDevice PanelNine { get; private set; }
        public static PanelDevice PanelTen { get; private set; }

        static ExternalTouchPanelMock()
        {
            PanelOne = new PanelDevice(0x03, true, "Panel One");
            PanelTwo = new PanelDevice(0x04, true, "Panel Two");
            PanelThree = new PanelDevice(0x05, true, "Panel Three");
            PanelFour = new PanelDevice(0x06, true, "Panel Four");
            PanelFive = new PanelDevice(0x07, true, "Panel Five");
            PanelSix = new PanelDevice(0x08, true, "Panel Six");
            PanelSeven = new PanelDevice(0x09, true, "Panel Seven");
            PanelEight = new PanelDevice(0x0A, true, "Panel Eight");
            PanelNine = new PanelDevice(0x0B, true, "Panel Nine");
            PanelTen = new PanelDevice(0x0C, true, "Panel Ten");
        }

        public static PanelDevice[] CreatePanelArray(int numberOfPanels)
        {
            PanelDevice[] panels = new PanelDevice[numberOfPanels];

            for (int i = 0; i < numberOfPanels; i++)
            {
                panels[i] = new PanelDevice((uint)i + 3, true, $"Panel {i}");
            }

            return panels;
        }
    }
}
