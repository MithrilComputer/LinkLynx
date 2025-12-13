using LinkLynx.Core.CrestronWrappers;
using LinkLynx.Core.Interfaces.Utility.Debugging.Logging;

namespace LinkLynx.Core.Src.Implementations.Collections.Devices.Contexts
{
    /// <summary>
    /// A collection of TouchPanelDevice instances.
    /// </summary>
    internal class TouchPanelGroup
    {
        /// <summary>
        /// The list of TouchPanelDevice instances in the group.
        /// </summary>
        private readonly List<TouchPanelDevice> touchPanels = new List<TouchPanelDevice>();

        /// <summary>
        /// The logger instance for logging within the TouchPanelGroup.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// The TouchPanelGroup Constructor.
        /// </summary>
        public TouchPanelGroup(ILogger logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// The TouchPanelGroup Constructor.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public TouchPanelGroup(ILogger logger, IEnumerable<TouchPanelDevice> initialTouchPanels)
        {
            this.logger = logger;

            if (initialTouchPanels == null)
                throw new ArgumentNullException(nameof(initialTouchPanels), $"[TouchPanelGroup] Error: Attempted to initialize TouchPanelGroup with null initialTouchPanels list.");

            foreach (TouchPanelDevice touchPanel in initialTouchPanels)
            {
                AddTouchPanel(touchPanel);
            }
        }

        /// <summary>
        /// Adds a TouchPanelDevice to the group.
        /// </summary>
        public TouchPanelGroup AddTouchPanel(TouchPanelDevice touchPanel)
        {
            if (touchPanel == null)
            {
                logger.Log($"[TouchPanelGroup] Warning: Attempted to add null TouchPanelDevice to TouchPanelGroup.");
                return this;
            }
            if (touchPanels.Contains(touchPanel))
            {
                logger.Log($"[TouchPanelGroup] Warning: Attempted to add a TouchPanelDevice that already exists in TouchPanelGroup. Device ID: {touchPanel.IPID}, Device Name: {touchPanel.Name}");
                return this;
            }

            touchPanels.Add(touchPanel);

            return this;
        }

        /// <summary>
        /// Removes a TouchPanelDevice from the group.
        /// </summary>
        public TouchPanelGroup RemoveTouchPanel(TouchPanelDevice touchPanel)
        {
            if (touchPanel == null)
            {
                logger.Log($"[TouchPanelGroup] Warning: Attempted to remove null TouchPanelDevice from TouchPanelGroup.");
                return this;
            }
            if (!touchPanels.Contains(touchPanel))
            {
                logger.Log($"[TouchPanelGroup] Warning: Attempted to remove a TouchPanelDevice that does not exist in TouchPanelGroup. Device ID: {touchPanel.IPID}, Device Name: {touchPanel.Name}");
                return this;
            }

            touchPanels.Remove(touchPanel);

            return this;
        }
    }
}
