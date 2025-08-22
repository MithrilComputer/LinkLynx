using Crestron.SimplSharp;
using Crestron.SimplSharpPro.DeviceSupport;
using LinkLynx.Core.Utility.Debugging.Logging;
using System;
using System.Collections.Generic;

namespace LinkLynx.Core.Collections.Pools
{

    /// <summary>
    /// The logic pool class that manages the logic groups for each panel device.
    /// </summary>
    internal sealed class LogicGroupPool
    {
        /// <summary>
        /// The singleton instance of the class.
        /// </summary>
        private static readonly LogicGroupPool instance = new LogicGroupPool();

        /// <summary>
        /// The singleton instance of the class.
        /// </summary>
        public static LogicGroupPool Instance => instance;

        /// <summary>
        /// Class constructor
        /// </summary>
        internal LogicGroupPool() { } 

        private readonly Dictionary<uint, PanelLogicGroup> deviceLogicPool = 
            new Dictionary<uint, PanelLogicGroup>();

        /// <summary>
        /// Registers a panel device and initializes its logic group.
        /// </summary>
        /// <param name="device">The device to initialize</param>
        internal void RegisterPanel(BasicTriList device)
        {
            ConsoleLogger.Log($"[LogicGroupPool] Registering panel with ID: {device.ID}");

            if (!deviceLogicPool.ContainsKey(device.ID))
            {
                PanelLogicGroup panelLogic = new PanelLogicGroup(device);
                deviceLogicPool[device.ID] = panelLogic;

                panelLogic.InitializePageLogic(); // Optional: call init logic per panel
            } else
                throw new ArgumentException($"[LogicGroupPool] Error: Panel with ID {device.ID} is already registered.");
        }

        internal void UnregisterPanel(BasicTriList device)
        {
            ConsoleLogger.Log($"[LogicGroupPool] UnregisterPanel panel with ID: {device.ID}");

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
        internal PanelLogicGroup GetPanelLogicGroup(BasicTriList device)
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
        internal void InitializePanelLogic(BasicTriList device)
        {
            if (deviceLogicPool.TryGetValue(device.ID, out var panelLogic))
            {
                panelLogic.InitializePageLogic();
            }
            else
            {
                throw new KeyNotFoundException($"No logic group found for device {device.ID}");
            }
        }

        /// <summary>
        /// Clears the stored logic groups, should only be called on system shutdown
        /// </summary>
        internal void Clear()
        {
            deviceLogicPool.Clear();
        }
    }
}
