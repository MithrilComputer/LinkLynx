using Crestron.SimplSharpPro.DeviceSupport;
using System.Collections.Generic;
using LinkLynx.Core.Logic.Pages;
using LinkLynx.Core.Factories;

namespace LinkLynx.Core.Collections
{
    /// <summary>
    /// A Grouping of logic that is served to every new panel. Contains all the logic to run the panel.
    /// </summary>
    internal class PanelLogicGroup
    {
        /// <summary>
        /// The pool that contains all the logic within the panel.
        /// </summary>
        internal readonly IReadOnlyDictionary<ushort, PageLogicBase> pageLogicPool = new Dictionary<ushort, PageLogicBase>();

        /// <summary>
        /// PanelLogicGroup Constructor that asks the PageFactory for a copy of all the registered page logic's.
        /// </summary>
        /// <param name="panel">The panel that is bound to this logic group.</param>
        internal PanelLogicGroup(BasicTriList panel)
        {
            pageLogicPool = PageFactory.BuildPagesForPanel(panel);
        }

        /// <summary>
        /// Initializes all the logic on the panel. Can use to bring it to the default start state.
        /// </summary>
        internal void InitializePageLogic()
        {
            foreach (KeyValuePair<ushort, PageLogicBase> pair in pageLogicPool)
            {
                pair.Value.Initialize();
            }
        }

        /// <summary>
        /// Gets a local Page Logic bases on the given page ID.
        /// </summary>
        /// <param name="pageID">The page id to ask for</param>
        internal PageLogicBase GetPageLogicFromID(ushort pageID)
        {
            pageLogicPool.TryGetValue(pageID, out var logic);
            return logic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pageId"></param>
        /// <returns></returns>
        internal T GetPage<T>(ushort pageId) where T : PageLogicBase
        {
            if (pageLogicPool.TryGetValue(pageId, out var logic) && logic is T typedLogic)
                return typedLogic;

            return null;
        }
    }
}
