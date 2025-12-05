using Crestron.SimplSharpPro.DeviceSupport;
using LinkLynx.Core.CrestronWrappers;
using LinkLynx.Implementations.Collections.PanelContexts;

namespace LinkLynx.Core.Interfaces.Collections.Pools
{
    /// <summary>
    /// The ILogicGroupPool interface defines a contract for managing a collection of panel logic groups.
    /// </summary>
    public interface IPanelScriptGroupPool
    {
        /// <summary>
        /// Registers a panel device and associates it with a logic group.
        /// </summary>
        void RegisterPanel(TouchPanelDevice device);

        /// <summary>
        /// Unregisters a panel device and removes its associated logic group.
        /// </summary>
        void UnregisterPanel(TouchPanelDevice device);

        /// <summary>
        /// Gets the logic group associated with the specified panel device.
        /// </summary>
        PanelScriptGroup GetPanelLogicGroup(TouchPanelDevice device);

        /// <summary>
        /// Initializes the panel logic for the specified panel device.
        /// </summary>
        void InitializePanelLogic(TouchPanelDevice device);

        /// <summary>
        /// Sets the default settings for the specified panel device.
        /// </summary>
        void SetPanelDefaults(TouchPanelDevice device);
    }
}
