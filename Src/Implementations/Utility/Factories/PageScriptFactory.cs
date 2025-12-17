using LinkLynx.Core.CrestronWrappers;
using LinkLynx.Core.Interfaces.Collections.Registries;
using LinkLynx.Core.Interfaces.Utility.Factories;
using LinkLynx.Implementations.Collections.Pages.Logic;

namespace LinkLynx.Implementations.Utility.Factories
{
    /// <summary>
    /// The PageFactory is responsible for creating PageLogicBase instances for a given PanelDevice
    /// </summary>
    public class PageScriptFactory : IPageScriptFactory
    {
        private readonly IPanelScriptRegistry pageRegistry;

        /// <summary>
        /// The Factory for creating pages for a panel.
        /// </summary>
        public PageScriptFactory(IPanelScriptRegistry pageRegistry)
        {
            this.pageRegistry = pageRegistry;
        }

        /// <summary>
        /// Creates a list of new instantiated pages for a given panel.
        /// </summary>
        /// <param name="panel">The panel to assign the PageLogicBase's to.</param>
        public Dictionary<ushort, PageLogicScript> BuildPagesForPanel(TouchPanelDevice panel)
        {
            // The ushort is the page ID
            Dictionary<ushort, PageLogicScript> createdPages = new Dictionary<ushort, PageLogicScript>();

            // Get all the registered pages from the registry.
            // TODO (This needs to change later to per device)
            IReadOnlyDictionary<ushort, Func<TouchPanelDevice, PageLogicScript>> registeredPages 
                = pageRegistry.GetAllRegistries();

            foreach (KeyValuePair<ushort, Func<TouchPanelDevice, PageLogicScript>> pair in registeredPages)
            {
                PageLogicScript page = pair.Value(panel); // This is the Func<PanelDevice, PageLogicBase>

                createdPages.Add(pair.Key, page);
            }

            return createdPages;
        }
    }
}
