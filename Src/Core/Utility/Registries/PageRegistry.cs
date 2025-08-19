using Crestron.SimplSharpPro.DeviceSupport;
using LinkLynx.Core.Logic.Pages;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using LinkLynx.Core.Utility.Debugging.Logging;

[assembly: InternalsVisibleTo("LinkLynx.Tests")]

namespace LinkLynx.Core.Utility.Registries
{
    /// <summary>
    /// A global page registry to keep track of all the known pages.
    /// </summary>
    internal sealed class PageRegistry
    {
        /// <summary>
        /// The singleton instance of the class.
        /// </summary>
        private static readonly PageRegistry instance = new PageRegistry();

        /// <summary>
        /// The singleton instance of the class.
        /// </summary>
        internal static PageRegistry Instance => instance;

        /// <summary>
        /// Class constructor.
        /// </summary>
        internal PageRegistry() { }

        /// <summary>
        /// The dictionary that stores all the page types that are added to the program.
        /// </summary>
        internal readonly Dictionary<ushort, Func<BasicTriList, PageLogicBase>> pageRegistry = new Dictionary<ushort, Func<BasicTriList, PageLogicBase>>();

        /// <summary>
        /// Adds a new page to the global page registry.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <param name="pageLogic">The page logic reference.</param>
        /// <exception cref="ArgumentException"></exception>
        internal void RegisterPage(ushort pageId, Func<BasicTriList, PageLogicBase> pageLogic)
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
        internal Func<BasicTriList, PageLogicBase> GetPage(ushort pageId)
        {
            if (pageRegistry.TryGetValue(pageId, out Func<BasicTriList, PageLogicBase> page))
            {
                return page;
            }
            else
            {
                ConsoleLogger.Log($"[PageRegistry] Warning: Page Registry does not contain a page for PageID: {pageId}");
                return null;
            }
        }

        /// <summary>
        /// Gets the current set of registered pages.
        /// </summary>
        /// <returns>A dictionary of page IDs and their factory functions.</returns>
        internal IReadOnlyDictionary<ushort, Func<BasicTriList, PageLogicBase>> GetAllRegistries()
        {
            return pageRegistry;
        }

        /// <summary>
        /// Clears the stored page entries, should only be called on system shutdown
        /// </summary>
        internal void Clear()
        {
            pageRegistry.Clear();
        }
    }
}
