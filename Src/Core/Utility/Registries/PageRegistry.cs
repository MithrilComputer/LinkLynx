using Crestron.SimplSharp;
using Crestron.SimplSharpPro.DeviceSupport;
using LinkLynx.Core.Logic;
using System;
using System.Collections.Generic;

namespace LinkLynx.Core.Utility.Registries
{
    /// <summary>
    /// A global page registry to keep track of all the known pages.
    /// </summary>
    internal static class PageRegistry
    {
        /// <summary>
        /// The dictionary that stores all the page types that are added to the program.
        /// </summary>
        internal static readonly Dictionary<ushort, Func<BasicTriList, PageLogicBase>> pageRegistry = new Dictionary<ushort, Func<BasicTriList, PageLogicBase>>();

        /// <summary>
        /// Adds a new page to the global page registry.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <param name="pageLogic">The page logic reference.</param>
        /// <exception cref="ArgumentException"></exception>
        internal static void RegisterPage(ushort pageId, Func<BasicTriList, PageLogicBase> pageLogic)
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
        internal static Func<BasicTriList, PageLogicBase> GetPage(ushort pageId)
        {
            if (pageRegistry.TryGetValue(pageId, out Func<BasicTriList, PageLogicBase> page))
            {
                return page;
            }
            else
            {
                CrestronConsole.PrintLine($"[PageRegistry] Warning: Page Registry does not contain a page for PageID: {pageId}");
                return null;
            }
        }

        /// <summary>
        /// Gets the current set of registered pages.
        /// </summary>
        /// <returns>A dictionary of page IDs and their factory functions.</returns>
        internal static IReadOnlyDictionary<ushort, Func<BasicTriList, PageLogicBase>> GetAllRegistries()
        {
            return pageRegistry;
        }

        /// <summary>
        /// Clears the stored page entries, should only be called on system shutdown
        /// </summary>
        internal static void Clear()
        {
            pageRegistry.Clear();
        }
    }
}
