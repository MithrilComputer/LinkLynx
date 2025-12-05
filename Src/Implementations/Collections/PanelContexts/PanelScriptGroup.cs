using LinkLynx.Core.CrestronPOCOs;
using LinkLynx.Core.Logic.Pages;

namespace LinkLynx.Implementations.Collections.PanelContexts
{
    /// <summary>
    /// A group of page logic scripts instances assigned to a specific panel.
    /// </summary>
    public class PanelScriptGroup
    {
        /// <summary>
        /// The pool that contains all the <see cref="PageLogicScript"/>s constructed for the assigned panel.
        /// </summary>
        private readonly IReadOnlyDictionary<ushort, PageLogicScript> pageLogicPool;

        /// <summary>
        /// The panel that is assigned to this logic group.
        /// </summary>
        public TouchPanelDevice AssignedPanel { get; private set; }

        /// <summary>
        /// PanelLogicGroup Constructor that asks the PageFactory for a copy of all the registered page logic's.
        /// </summary>
        /// <param name="panel">The panel that is bound to this logic group.</param>
        /// <param name="pageLogicPool">The group of logic for the panel</param>
        public PanelScriptGroup(TouchPanelDevice panel, Dictionary<ushort, PageLogicScript> pageLogicPool)
        {
            this.pageLogicPool = pageLogicPool;
            AssignedPanel = panel;
        }

        /// <summary>
        /// Initializes all the logic on the panel once at runtime.
        /// </summary>
        public void InitializePageLogic()
        {
            foreach (KeyValuePair<ushort, PageLogicScript> pair in pageLogicPool)
            {
                pair.Value.Initialize();
            }
        }

        /// <summary>
        /// Sets all the page's logic and  to the default start state.
        /// </summary>
        public void SetPageDefaults()
        {
            foreach (KeyValuePair<ushort, PageLogicScript> pair in pageLogicPool)
            {
                pair.Value.SetDefaults();
            }
        }

        /// <summary>
        /// Gets a local Page Logic bases on the given page ID.
        /// </summary>
        /// <param name="pageId">The page id to ask for</param>
        public PageLogicScript GetPageLogicFromId(ushort pageId)
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
        public T GetPage<T>(ushort pageId) where T : PageLogicScript
        {
            if (pageLogicPool.TryGetValue(pageId, out var logic) && logic is T typedLogic)
                return typedLogic;

            return null;
        }
    }
}
