using LinkLynx.Core.CrestronPOCOs;
using LinkLynx.Core.Interfaces.Collections.Pools;
using LinkLynx.Core.Interfaces.Utility.Debugging.Logging;

namespace LinkLynx.Implementations.Collections.Pools
{
    /// <summary>
    /// Manages a collection of panel devices, allowing for the retrieval, addition, and lifecycle management of panels.
    /// </summary>
    /// <remarks>The <see cref="PanelPool"/> class provides functionality to register and retrieve panel
    /// devices by their unique identifiers. It ensures that duplicate panel IDs are not added to the collection and
    /// logs relevant messages for operations. This class implements <see cref="IDisposable"/> to allow proper cleanup
    /// of resources when the pool is no longer needed.</remarks>
    public sealed class PanelPool : IPanelPool, IDisposable
    {
        private readonly ILogger consoleLogger;

        private readonly Dictionary<uint, TouchPanelDevice> panels = new Dictionary<uint, TouchPanelDevice>();

        /// <summary>
        /// Initializes a new instance of the <see cref="PanelPool"/> class.
        /// </summary>
        /// <param name="logger">The logger instance used to log messages and events related to the panel pool.</param>
        public PanelPool(ILogger logger)
        {
            consoleLogger = logger;
        }

        /// <summary>
        /// Gets the panel device associated with the specified panel ID.
        /// </summary>
        /// <param name="panelId">The PanelID to attempt to retrieve</param>
        /// <returns>The Panel Added with the ID</returns>
        /// <exception cref="KeyNotFoundException"> Is thrown when the ID cant be found.</exception>
        public TouchPanelDevice GetPanel(uint panelId)
        {
            if (panels.TryGetValue(panelId, out TouchPanelDevice panel))
            {
                return panel;
            }
            throw new KeyNotFoundException($"[PanelPool] Panel with ID {panelId} not found. Did you register it?");
        }

        /// <summary>
        /// Attempts to add a panel to the collection with the specified panel ID.
        /// </summary>
        /// <remarks>This method ensures that no duplicate panel IDs are added to the collection. If a
        /// panel with the specified ID already exists, the method logs a message and returns <see
        /// langword="false"/>.</remarks>
        /// <param name="panelId">The unique identifier for the panel to add.</param>
        /// <param name="panel">The <see cref="TouchPanelDevice"/> instance to associate with the specified panel ID.</param>
        /// <returns><see langword="true"/> if the panel was successfully added; otherwise, <see langword="false"/> if a panel
        /// with the specified ID already exists in the collection.</returns>
        public bool TryAddPanel(uint panelId, TouchPanelDevice panel)
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

        /// <inheritdoc/>
        public void Dispose()
        {
            panels.Clear();
        }
    }
}
