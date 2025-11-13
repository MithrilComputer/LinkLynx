using Crestron.SimplSharpPro.DeviceSupport;
using LinkLynx.Core.CrestronPOCOs;
using LinkLynx.Implementations.Collections.PanelContexts;

namespace LinkLynx.Core.Interfaces.Collections.Pools
{
    /// <summary>
    /// The ILogicGroupPool interface defines a contract for managing a collection of panel logic groups.
    /// </summary>
    public interface ILogicGroupPool
    {
        /// <summary>
        /// Registers a panel device and associates it with a logic group.
        /// </summary>
        void RegisterPanel(PanelDevice device);

        /// <summary>
        /// Unregisters a panel device and removes its associated logic group.
        /// </summary>
        void UnregisterPanel(PanelDevice device);

        /// <summary>
        /// Gets the logic group associated with the specified panel device.
        /// </summary>
        PanelLogicGroup GetPanelLogicGroup(PanelDevice device);

        /// <summary>
        /// Initializes the panel logic for the specified panel device.
        /// </summary>
        void InitializePanelLogic(PanelDevice device);

        /// <summary>
        /// Sets the default settings for the specified panel device.
        /// </summary>
        void SetPanelDefaults(PanelDevice device);
    }
}
