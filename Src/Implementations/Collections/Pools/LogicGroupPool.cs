using Crestron.SimplSharpPro.DeviceSupport;
using LinkLynx.Core.Interfaces.Collections.Pools;
using LinkLynx.Core.Interfaces.Utility.Debugging.Logging;
using LinkLynx.Core.Src.Core.Interfaces.Utility.Factories;
using LinkLynx.Implementations.Collections.PanelContexts;
using System;
using System.Collections.Generic;

namespace LinkLynx.Implementations.Collections.Pools
{
    /// <summary>
    /// The logic pool class that manages the logic groups for each panel device.
    /// </summary>
    internal sealed class LogicGroupPool : ILogicGroupPool, IDisposable
    { 
        private readonly ILogger consoleLogger;
        private readonly IPageFactory pageFactory;

        /// <summary>
        /// Class constructor
        /// </summary>
        public LogicGroupPool(ILogger consoleLogger, IPageFactory pageFactory) 
        { 
            this.consoleLogger = consoleLogger;
            this.pageFactory = pageFactory;
        } 

        private readonly Dictionary<uint, PanelLogicGroup> deviceLogicPool = 
            new Dictionary<uint, PanelLogicGroup>();

        /// <summary>
        /// Registers a panel device and initializes its logic group.
        /// </summary>
        /// <param name="panel">The device to initialize</param>
        public void RegisterPanel(BasicTriList panel)
        {
            if(panel == null)
                throw new ArgumentNullException(nameof(panel));

            uint id = panel.ID;

            if (id == 0)
                throw new ArgumentException("[LogicGroupPool] Panel.ID is 0 (invalid/uninitialized).", nameof(panel));

            if (!deviceLogicPool.ContainsKey(panel.ID))
            {
                consoleLogger.Log($"[LogicGroupPool] Registering Panel with ID: {panel.ID}");

                PanelLogicGroup panelLogic;

                try
                {
                    panelLogic = new PanelLogicGroup(panel, pageFactory.BuildPagesForPanel(panel)); // Todo make a factory
                }
                catch (Exception ex)
                {
                    consoleLogger.Log($"[LogicGroupPool] PanelLogicGroup ctor failed for ID {id}: {ex.GetType().Name}: {ex.Message}");
                    throw; // rethrow so you see the real stack
                }

                try
                {
                    deviceLogicPool.Add(id, panelLogic);
                }
                catch (Exception ex)
                {
                    consoleLogger.Log($"[LogicGroupPool] Failed adding panel ID {id} to pool: {ex.GetType().Name}: {ex.Message}");
                    throw;
                }

                consoleLogger.Log($"[LogicGroupPool] Panel with ID: {panel.ID} registered successfully!");
            } else
                throw new ArgumentException($"[LogicGroupPool] Error: Panel with ID {panel.ID} is already registered.");
        }

        public void UnregisterPanel(BasicTriList device)
        {
            consoleLogger.Log($"[LogicGroupPool] UnregisterPanel panel with ID: {device.ID}");

            if(deviceLogicPool.ContainsKey(device.ID))
            {
                deviceLogicPool[device.ID] = null;
            } else
                throw new ArgumentException($"[LogicGroupPool] Error: Panel with ID: failed to Unregister due to no registry being present.");
        }

        /// <summary>
        /// Gets the logic group for a specific panel.
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public PanelLogicGroup GetPanelLogicGroup(BasicTriList device)
        {
            if (deviceLogicPool.TryGetValue(device.ID, out var panelLogic))
            {
                return panelLogic;
            }
            else
            {
                throw new KeyNotFoundException($"No logic group found for device {device.ID}");
            }
        }

        /// <summary>
        /// Initializes the logic for a specific panel device.
        /// </summary>
        /// <param name="device"></param>
        /// <exception cref="KeyNotFoundException"></exception>
        public void InitializePanelLogic(BasicTriList device)
        {
            if (deviceLogicPool.TryGetValue(device.ID, out PanelLogicGroup panelLogic))
            {
                panelLogic.InitializePageLogic();
            }
            else
            {
                throw new KeyNotFoundException($"No logic group found for device {device.ID}");
            }
        }


        /// <summary>
        /// Initializes the logic for a specific panel device.
        /// </summary>
        /// <param name="device"></param>
        /// <exception cref="KeyNotFoundException"></exception>
        public void SetPanelDefaults(BasicTriList device)
        {
            if (deviceLogicPool.TryGetValue(device.ID, out PanelLogicGroup panelLogic))
            {
                panelLogic.SetPageDefaults();
            }
            else
            {
                throw new KeyNotFoundException($"No logic group found for device {device.ID}");
            }
        }

        /// <summary>
        /// Clears the stored logic groups, should only be called on system shutdown
        /// </summary>
        public void Dispose()
        {
            deviceLogicPool.Clear();
        }
    }
}
