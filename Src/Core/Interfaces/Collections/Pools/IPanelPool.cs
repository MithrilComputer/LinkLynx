using LinkLynx.Core.CrestronPOCOs;

namespace LinkLynx.Core.Interfaces.Collections.Pools
{
    /// <summary>
    /// Represents a pool of panel devices that can be retrieved or added by their unique identifiers.
    /// </summary>
    public interface IPanelPool
    {
        /// <summary>
        /// Retrieves the panel device associated with the specified panel ID.
        /// </summary>
        PanelDevice GetPanel(uint panelId);

        /// <summary>
        /// Attempts to add a panel to the collection using the specified panel ID.
        /// </summary>
        /// <param name="panelId">The unique IPID for the panel to be keyed to.</param>
        /// <param name="panel">The <see cref="PanelDevice"/> instance representing the panel to add.</param>
        bool TryAddPanel(uint panelId, PanelDevice panel);
    }
}
