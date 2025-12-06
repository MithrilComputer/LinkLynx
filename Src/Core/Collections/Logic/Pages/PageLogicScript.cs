using LinkLynx.Core.CrestronWrappers;

namespace LinkLynx.Core.Logic.Pages
{
    /// <summary>
    /// This is the base class for all panel specific logic, Represents a page's worth of logic.
    /// </summary>
    public abstract class PageLogicScript
    {
        /// <summary>
        /// The panel assigned to the page.
        /// </summary>
        public TouchPanelDevice OwnerPanel { get; }

        /// <summary>
        /// Constructs a new page, takes in a panel that acts as the host device.
        /// </summary>
        /// <param name="panel">The panel that is connected to this page's logic.</param>
        public PageLogicScript(TouchPanelDevice panel)
        {
            OwnerPanel = panel;
        }

        /// <summary>
        /// This sets all the page logic values to default.
        /// </summary>
        public virtual void Initialize() { }

        /// <summary>
        /// This sets all the page logic values to default.
        /// </summary>
        public abstract void SetDefaults();
    }
}
