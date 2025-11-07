using Crestron.SimplSharpPro.DeviceSupport;
using LinkLynx.Core.Interfaces.Collections.Registries;
using LinkLynx.Core.Logic.Pages;
using LinkLynx.Core.Interfaces.Utility.Factories;
using System;
using System.Collections.Generic;
using LinkLynx.Core.CrestronPOCOs;

namespace LinkLynx.Implementations.Utility.Factories
{
    public class PageFactory : IPageFactory
    {
        private IPageRegistry pageRegistry;

        public PageFactory(IPageRegistry pageRegistry)
        {
            this.pageRegistry = pageRegistry;
        }

        /// <summary>
        /// Creates a list of new instantiated pages for a given panel.
        /// </summary>
        /// <param name="panel">The panel to assign the PageLogicBase's to.</param>
        /// <returns></returns>
        public Dictionary<ushort, PageLogicBase> BuildPagesForPanel(PanelDevice panel)
        {
            Dictionary<ushort, PageLogicBase> createdPages = new Dictionary<ushort, PageLogicBase>();

            // Get all the registered pages from the registry.
            // TODO (This needs to change later to per device)
            IReadOnlyDictionary<ushort, Func<PanelDevice, PageLogicBase>> registeredPages 
                = pageRegistry.GetAllRegistries();

            foreach (KeyValuePair<ushort, Func<PanelDevice, PageLogicBase>> pair in registeredPages)
            {
                PageLogicBase page = pair.Value(panel); // This is the Func<PanelDevice, PageLogicBase>

                createdPages.Add(pair.Key, page);
            }

            return createdPages;
        }
    }
}
