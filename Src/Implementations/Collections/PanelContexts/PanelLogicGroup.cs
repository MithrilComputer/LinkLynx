using Crestron.SimplSharpPro.DeviceSupport;
using System.Collections.Generic;
using LinkLynx.Core.Logic.Pages;

namespace LinkLynx.Implementations.Collections.PanelContexts
{
    /// <summary>
    /// A Grouping of logic that is served to every new panel. Contains all the logic to run the panel.
    /// </summary>
    public class PanelLogicGroup
    {
        /// <summary>
        /// The pool that contains all the logic within the panel.
        /// </summary>
        private readonly IReadOnlyDictionary<ushort, PageLogicBase> pageLogicPool = new Dictionary<ushort, PageLogicBase>();

        /// <summary>
        /// PanelLogicGroup Constructor that asks the PageFactory for a copy of all the registered page logic's.
        /// </summary>
        /// <param name="panel">The panel that is bound to this logic group.</param>
        /// <param name="pageLogicPool">The group of logic for the panel</param>
        internal PanelLogicGroup(BasicTriList panel, Dictionary<ushort, PageLogicBase> pageLogicPool)
        {
            this.pageLogicPool = pageLogicPool;
        }

        /// <summary>
        /// Initializes all the logic on the panel once at runtime.
        /// </summary>
        internal void InitializePageLogic()
        {
            foreach (KeyValuePair<ushort, PageLogicBase> pair in pageLogicPool)
            {
                pair.Value.Initialize();
            }
        }

        /// <summary>
        /// Sets all the page's logic and  to the default start state.
        /// </summary>
        internal void SetPageDefaults()
        {
            foreach (KeyValuePair<ushort, PageLogicBase> pair in pageLogicPool)
            {
                pair.Value.SetDefaults();
            }
        }

        /// <summary>
        /// Gets a local Page Logic bases on the given page ID.
        /// </summary>
        /// <param name="pageId">The page id to ask for</param>
        internal PageLogicBase GetPageLogicFromId(ushort pageId)
        {
            pageLogicPool.TryGetValue(pageId, out var logic);
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
