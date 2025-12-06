using LinkLynx.Core.CrestronWrappers;
using LinkLynx.Core.Interfaces.Collections.Registries;
using LinkLynx.Core.Interfaces.Utility.Debugging.Logging;
using LinkLynx.Implementations.Collections.Pages.Logic;

namespace LinkLynx.Implementations.Collections.Registries
{
    /// <summary>
    /// A global page registry to keep track of all the known pages.
    /// </summary>
    public sealed class PageScriptRegistry : IPageScriptRegistry, IDisposable
    {
        private readonly ILogger consoleLogger;

        /// <summary>
        /// Class constructor.
        /// </summary>
        public PageScriptRegistry(ILogger consoleLogger) 
        {
            this.consoleLogger = consoleLogger;
        }

        /// <summary>
        /// The dictionary that stores all the page types that are added to the program.
        /// </summary>
        private readonly Dictionary<ushort, Func<TouchPanelDevice, PageLogicScript>> pageRegistry = new Dictionary<ushort, Func<TouchPanelDevice, PageLogicScript>>();


        /// <summary>
        /// Adds a new page to the global page registry.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <param name="pageLogic">The page logic reference.</param>
        /// <exception cref="ArgumentException"></exception>
        public void RegisterPage(ushort pageId, Func<TouchPanelDevice, PageLogicScript> pageLogic)
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
        public Func<TouchPanelDevice, PageLogicScript> GetPage(ushort pageId)
        {
            if (pageRegistry.TryGetValue(pageId, out Func<TouchPanelDevice, PageLogicScript> page))
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
        public IReadOnlyDictionary<ushort, Func<TouchPanelDevice, PageLogicScript>> GetAllRegistries()
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
