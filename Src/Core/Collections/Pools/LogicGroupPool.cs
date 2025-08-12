using Crestron.SimplSharp;
using Crestron.SimplSharpPro.DeviceSupport;
using System;
using System.Collections.Generic;

namespace LinkLynx.Core.Collections.Pools
{

    /// <summary>
    /// The logic pool class that manages the logic groups for each panel device.
    /// </summary>
    internal static class LogicGroupPool
    {
        private static readonly Dictionary<uint, PanelLogicGroup> deviceLogicMap = 
            new Dictionary<uint, PanelLogicGroup>();

        /// <summary>
        /// Registers a panel device and initializes its logic group.
        /// </summary>
        /// <param name="device">The device to initialize</param>
        internal static void RegisterPanel(BasicTriList device)
        {
            CrestronConsole.PrintLine($"Registering panel with ID: {device.ID}");

            if (!deviceLogicMap.ContainsKey(device.ID))
            {
                PanelLogicGroup panelLogic = new PanelLogicGroup(device);
                deviceLogicMap[device.ID] = panelLogic;

                panelLogic.InitializePageLogic(); // Optional: call init logic per panel
            } else
                throw new ArgumentException($"Panel with ID {device.ID} is already registered.");
        }

        /// <summary>
        /// Gets the logic group for a specific panel.
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        internal static PanelLogicGroup GetPanelLogicGroup(BasicTriList device)
        {
            if (deviceLogicMap.TryGetValue(device.ID, out var panelLogic))
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
        internal static void InitializePanelLogic(BasicTriList device)
        {
            if (deviceLogicMap.TryGetValue(device.ID, out var panelLogic))
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
        internal static void Clear()
        {
            deviceLogicMap.Clear();
        }
    }
}
