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
            if(device == null)
                throw new ArgumentNullException(nameof(device));

            uint id = device.ID;
            if (id == 0)
                throw new ArgumentException("[LogicGroupPool] Device.ID is 0 (invalid/uninitialized).", nameof(device));


            ConsoleLogger.Log($"[LogicGroupPool] Registering panel with ID: {device.ID}");

            if (!deviceLogicPool.ContainsKey(device.ID))
            {

                ConsoleLogger.Log($"[LogicGroupPool] Creatasdas dA D");

                PanelLogicGroup panelLogic;

                try
                {
                    panelLogic = new PanelLogicGroup(device);
                }
                catch (Exception ex)
                {
                    ConsoleLogger.Log($"[LogicGroupPool] PanelLogicGroup ctor failed for ID {id}: {ex.GetType().Name}: {ex.Message}");
                    throw; // rethrow so you see the real stack
                }

                try
                {
                    deviceLogicPool.Add(id, panelLogic);
                }
                catch (Exception ex)
                {
                    ConsoleLogger.Log($"[LogicGroupPool] Failed adding panel ID {id} to pool: {ex.GetType().Name}: {ex.Message}");
                    throw;
                }

                ConsoleLogger.Log($"[LogicGroupPool] Panel with ID: {device.ID} registered successfully!");
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
        internal void SetPanelDefaults(BasicTriList device)
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
        internal void Clear()
        {
            deviceLogicPool.Clear();
        }
    }
}
