using Crestron.SimplSharpPro.DeviceSupport;
using LinkLynx.Core.CrestronPOCOs;
using LinkLynx.Core.Interfaces.Collections.Registries;
using LinkLynx.Core.Interfaces.Utility.Debugging.Logging;
using LinkLynx.Core.Logic.Pages;
using System;
using System.Collections.Generic;

namespace LinkLynx.Implementations.Collections.Registries
{
    /// <summary>
    /// A global page registry to keep track of all the known pages.
    /// </summary>
    public sealed class PageRegistry : IPageRegistry, IDisposable
    {
        private readonly ILogger consoleLogger;

        /// <summary>
        /// Class constructor.
        /// </summary>
        public PageRegistry(ILogger consoleLogger) 
        {
            this.consoleLogger = consoleLogger;
        }

        /// <summary>
        /// The dictionary that stores all the page types that are added to the program.
        /// </summary>
        private readonly Dictionary<ushort, Func<PanelDevice, PageLogicBase>> pageRegistry = new Dictionary<ushort, Func<PanelDevice, PageLogicBase>>();

        /// <summary>
        /// Adds a new page to the global page registry.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <param name="pageLogic">The page logic reference.</param>
        /// <exception cref="ArgumentException"></exception>
        public void RegisterPage(ushort pageId, Func<PanelDevice, PageLogicBase> pageLogic)
        {
            if (pageRegistry.ContainsKey(pageId))
            {
                throw new ArgumentException($"[PageRegistry] Error: Page Registry already contains a key for PageID: {pageId}");
            }
            else
            {
                pageRegistry.Add(pageId, pageLogic);
            }
        }

        /// <summary>
        /// Gets a page logic type based on its pageID.
        /// </summary>
        /// <param name="pageId">The id of the page to get.</param>
        /// <returns>A Func that represents the page logic that is stored with the key.</returns>
        public Func<PanelDevice, PageLogicBase> GetPage(ushort pageId)
        {
            if (pageRegistry.TryGetValue(pageId, out Func<PanelDevice, PageLogicBase> page))
            {
                return page;
            }
            else
            {
                consoleLogger.Log($"[PageRegistry] Warning: Page Registry does not contain a page for PageID: {pageId}");
                return null;
            }
        }

        /// <summary>
        /// Gets the current set of registered pages.
        /// </summary>
        /// <returns>A dictionary of page IDs and their factory functions.</returns>
        public IReadOnlyDictionary<ushort, Func<PanelDevice, PageLogicBase>> GetAllRegistries()
        {
            return pageRegistry;
        }

        /// <summary>
        /// Clears the stored page entries, should only be called on system shutdown
        /// </summary>
        public void Dispose()
        {
            pageRegistry.Clear();
        }
    }
}
