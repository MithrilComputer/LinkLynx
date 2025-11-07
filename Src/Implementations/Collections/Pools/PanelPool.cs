using LinkLynx.Core.CrestronPOCOs;
using LinkLynx.Core.Interfaces.Collections.Pools;
using LinkLynx.Core.Interfaces.Utility.Debugging.Logging;
using System;
using System.Collections.Generic;

namespace LinkLynx.Implementations.Collections.Pools
{
    public class PanelPool : IPanelPool, IDisposable
    {
        private readonly ILogger consoleLogger;

        private Dictionary<uint, PanelDevice> panels = new Dictionary<uint, PanelDevice>();

        public PanelPool(ILogger logger)
        {
            consoleLogger = logger;
        }

        public PanelDevice GetPanel(uint panelId)
        {
            if (panels.TryGetValue(panelId, out var panel))
            {
                return panel;
            }
            throw new KeyNotFoundException($"[PanelPool] Panel with ID {panelId} not found. Did you register it?");
        }

        public bool TryAddPanel(uint panelId, PanelDevice panel)
        {
            if (panels.ContainsKey(panelId))
            {
                consoleLogger.Log($"[PanelPool] Panel with ID {panelId} is already registered.");
                return false;
            }

            consoleLogger.Log($"[PanelPool] Added panel with ID {panelId}");
            panels[panelId] = panel;
            return true;
        }

        public void Dispose()
        {
            panels.Clear();
        }
    }
}
