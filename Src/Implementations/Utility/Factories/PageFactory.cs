using LinkLynx.Core.Interfaces.Collections.Registries;
using LinkLynx.Core.Logic.Pages;
using LinkLynx.Core.Interfaces.Utility.Factories;
using LinkLynx.Core.CrestronPOCOs;

namespace LinkLynx.Implementations.Utility.Factories
{
    /// <summary>
    /// The PageFactory is responsible for creating PageLogicBase instances for a given PanelDevice
    /// </summary>
    public class PageFactory : IPageFactory
    {
        private IPageRegistry pageRegistry;

        /// <summary>
        /// The Factory for creating pages for a panel.
        /// </summary>
        /// <param name="pageRegistry"></param>
        public PageFactory(IPageRegistry pageRegistry)
        {
            this.pageRegistry = pageRegistry;
        }

        /// <summary>
        /// Creates a list of new instantiated pages for a given panel.
        /// </summary>
        /// <param name="panel">The panel to assign the PageLogicBase's to.</param>
        /// <returns></returns>
        public Dictionary<ushort, PageLogicBlock> BuildPagesForPanel(PanelDevice panel)
        {
            Dictionary<ushort, PageLogicBlock> createdPages = new Dictionary<ushort, PageLogicBlock>();

            // Get all the registered pages from the registry.
            // TODO (This needs to change later to per device)
            IReadOnlyDictionary<ushort, Func<PanelDevice, PageLogicBlock>> registeredPages 
                = pageRegistry.GetAllRegistries();

            foreach (KeyValuePair<ushort, Func<PanelDevice, PageLogicBlock>> pair in registeredPages)
            {
                PageLogicBlock page = pair.Value(panel); // This is the Func<PanelDevice, PageLogicBase>

                createdPages.Add(pair.Key, page);
            }

            return createdPages;
        }
    }
}
