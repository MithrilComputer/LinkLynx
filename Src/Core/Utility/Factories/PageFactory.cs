using Crestron.SimplSharpPro.DeviceSupport;
using LinkLynx.Core.Logic.Pages;
using LinkLynx.Core.Utility.Registries;
using System;
using System.Collections.Generic;

namespace LinkLynx.Core.Factories
{
    internal static class PageFactory
    {
        /// <summary>
        /// Creates a list of new instantiated pages for a given panel.
        /// </summary>
        /// <param name="panel">The panel to assign the PageLogicBase's to.</param>
        /// <returns></returns>
        internal static Dictionary<ushort, PageLogicBase> BuildPagesForPanel(BasicTriList panel)
        {
            Dictionary<ushort, PageLogicBase> createdPages = new Dictionary<ushort, PageLogicBase>();

            IReadOnlyDictionary<ushort, Func<BasicTriList, PageLogicBase>> registeredPages = PageRegistry.GetAllRegistries();

            foreach (KeyValuePair<ushort, Func<BasicTriList, PageLogicBase>> pair in registeredPages)
            {
                PageLogicBase page = pair.Value(panel); // This is the Func<BasicTriList, PageLogicBase>

                createdPages.Add(pair.Key, page);
            }

            return createdPages;
        }
    }
}
