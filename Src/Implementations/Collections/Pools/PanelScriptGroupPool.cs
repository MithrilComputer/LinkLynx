using LinkLynx.Core.CrestronWrappers;
using LinkLynx.Core.Interfaces.Collections.Pools;
using LinkLynx.Core.Interfaces.Utility.Debugging.Logging;
using LinkLynx.Core.Interfaces.Utility.Factories;
using LinkLynx.Core.Src.Core.Interfaces.Utility.Factories;
using LinkLynx.Core.Src.Implementations.Utility.Factories;
using LinkLynx.Implementations.Collections.Pages.Contexts;

namespace LinkLynx.Implementations.Collections.Pools
{
    /// <summary>
    /// The logic pool class that manages the logic groups for each panel device.
    /// </summary>
    public sealed class PanelScriptGroupPool : IPanelScriptGroupPool, IDisposable
    {
        private readonly ILogger consoleLogger;
        private readonly IPageScriptFactory pageFactory;
        private readonly IPanelScriptGroupFactory panelScriptGroupFactory;

        /// <summary>
        /// Class constructor
        /// </summary>
        public PanelScriptGroupPool(ILogger consoleLogger, IPageScriptFactory pageFactory, IPanelScriptGroupFactory panelScriptGroupFactory)
        {
            this.consoleLogger = consoleLogger;
            this.pageFactory = pageFactory;
            this.panelScriptGroupFactory = panelScriptGroupFactory;
        }

        private readonly Dictionary<uint, PanelScriptGroup> panelScriptPool = 
            new Dictionary<uint, PanelScriptGroup>();

        /// <summary>
        /// Registers a Panel Device and initializes its logic group.
        /// </summary>
        /// <param name="device">The Device to initialize</param>
        public void RegisterPanel(TouchPanelDevice device)
        {
            if(device == null)
                throw new ArgumentNullException(nameof(device));

            uint id = device.IPID;

            if (id == 0)
                throw new ArgumentException("[PanelScriptGroupPool] Panel.ID is 0 (invalid/uninitialized).", nameof(device));

            if (!panelScriptPool.ContainsKey(device.IPID))
            {
                consoleLogger.Log($"[PanelScriptGroupPool] Registering Panel with ID: {device.IPID}");

                PanelScriptGroup panelLogic;

                try
                {
                    panelLogic = panelScriptGroupFactory.CreateNewLogicGroup(device, pageFactory.BuildPagesForPanel(device));
                }
                catch (Exception ex)
                {
                    consoleLogger.Log($"[PanelScriptGroupPool] PanelLogicGroup ctor failed for ID {id}: {ex.GetType().Name}: {ex.Message}");
                    throw; // rethrow so you see the real stack
                }

                try
                {
                    device.SetLoadedScripts(panelLogic);
                    panelScriptPool.Add(id, panelLogic);
                }
                catch (Exception ex)
                {
                    consoleLogger.Log($"[PanelScriptGroupPool] Failed adding panel ID {id} to pool: {ex.GetType().Name}: {ex.Message}");
                    throw;
                }

                consoleLogger.Log($"[PanelScriptGroupPool] Panel with ID: {device.IPID} registered successfully!");
            }
            else
            {
                throw new ArgumentException($"[PanelScriptGroupPool] Error: Panel with ID {device.IPID} is already registered.");
            }
        }

        /// <summary>
        /// Unregisters a panel associated with the specified device.
        /// </summary>
        /// <remarks>This method removes the association of the panel with the given device. If the device
        /// is not registered, an exception is thrown.</remarks>
        /// <param name="device">The device whose associated panel is to be unregistered. The device must have a valid registry entry.</param>
        /// <exception cref="ArgumentException">Thrown if the specified device does not have a registry entry.</exception>
        public void UnregisterPanel(TouchPanelDevice device)
        {
            consoleLogger.Log($"[PanelScriptGroupPool] UnregisterPanel panel with ID: {device.IPID}");

            if(panelScriptPool.ContainsKey(device.IPID))
            {
                panelScriptPool.Remove(device.IPID);
            } else
                throw new ArgumentException($"[PanelScriptGroupPool] Error: Panel with ID: failed to Unregister due to no registry being present.");
        }

        /// <summary>
        /// Gets the logic group for a specific panel.
        /// </summary>
        /// <exception cref="KeyNotFoundException"></exception>
        public PanelScriptGroup GetPanelLogicGroup(TouchPanelDevice device)
        {
            if (panelScriptPool.TryGetValue(device.IPID, out PanelScriptGroup panelLogic))
            {
                return panelLogic;
            }
            else
            {
                throw new KeyNotFoundException($"[PanelScriptGroupPool] Error: No logic group found for device {device.IPID}");
            }
        }

        /// <summary>
        /// Initializes the logic for a specific panel device.
        /// </summary>
        /// <param name="device"></param>
        /// <exception cref="KeyNotFoundException"></exception>
        public void InitializePanelLogic(TouchPanelDevice device)
        {
            if (panelScriptPool.TryGetValue(device.IPID, out PanelScriptGroup panelLogic))
            {
                panelLogic.InitializePageLogic();
            }
            else
            {
                throw new KeyNotFoundException($"No logic group found for device {device.IPID}");
            }
        }


        /// <summary>
        /// Initializes the logic for a specific panel device.
        /// </summary>
        /// <param name="device"></param>
        /// <exception cref="KeyNotFoundException"></exception>
        public void SetPanelDefaults(TouchPanelDevice device)
        {
            if (panelScriptPool.TryGetValue(device.IPID, out PanelScriptGroup panelLogic))
            {
                panelLogic.SetPageDefaults();
            }
            else
            {
                throw new KeyNotFoundException($"No logic group found for device {device.IPID}");
            }
        }

        /// <summary>
        /// Clears the stored logic groups, should only be called on system shutdown
        /// </summary>
        public void Dispose()
        {
            panelScriptPool.Clear();
        }
    }
}
