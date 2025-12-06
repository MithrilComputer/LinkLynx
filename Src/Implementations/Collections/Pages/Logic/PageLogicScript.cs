using LinkLynx.Core.CrestronWrappers;

namespace LinkLynx.Implementations.Collections.Pages.Logic
{
    /// <summary>
    /// This is the base class for all panel specific logic, Represents a page's worth of logic.
    /// </summary>
    public abstract class PageLogicScript
    {
        /// <summary>
        /// The panel assigned to the page.
        /// </summary>
        public TouchPanelDevice OwnerPanel { get; private set; }

        /// <summary>
        /// Constructs a new page, takes in a panel that acts as the host device.
        /// </summary>
        /// <param name="panel">The panel that is connected to this page's logic.</param>
        public PageLogicScript(TouchPanelDevice panel)
        {
            OwnerPanel = panel;
        }

        /// <summary>
        /// Allows for initialization of the page logic at runtime.
        /// </summary>
        public virtual void Initialize() { }

        /// <summary>
        /// Allows for setting the page logic to default states.
        /// </summary>
        public abstract void SetDefaults();
    }
}
