using LinkLynx.Core.CrestronWrappers;
using LinkLynx.Implementations.Collections.Pages.Logic;

namespace LinkLynx.Implementations.Collections.Pages.Contexts
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
        public PageLogicScript GetScriptLogicFromId(ushort pageId)
        {
            pageLogicPool.TryGetValue(pageId, out var logic);
            return logic;
        }

        /// <summary>
        /// Gets a typed page logic script from the pool.
        /// </summary>
        /// <typeparam name="T">The type you're attempting to get.</typeparam>
        /// <param name="pageId">The pageId of the Type your trying to get.</param>
        public T GetScriptFromTypeAndID<T>(ushort pageId) where T : PageLogicScript
        {
            if (pageLogicPool.TryGetValue(pageId, out var logic) && logic is T typedLogic)
                return typedLogic;

            return null;
        }

        /// <summary>
        /// Gets the first typed page logic script from the pool.
        /// </summary>
        /// <typeparam name="T">The type you're attempting to get.</typeparam>
        public T GetScriptFromType<T>() where T : PageLogicScript
        {
            foreach (PageLogicScript logic in pageLogicPool.Values)
            {
                if (logic is T typedLogic)
                    return typedLogic;
            }

            return null;
        }

        /// <summary>
        /// Gets all typed page logic script from the pool given a type.
        /// </summary>
        /// <typeparam name="T">The type you're attempting to get.</typeparam>
        public List<T> GetScriptsFromType<T>() where T : PageLogicScript
        {
            List<T> values = new List<T>();

            foreach (PageLogicScript logic in pageLogicPool.Values)
            {
                if (logic is T typedLogic)
                    values.Add(typedLogic);
            }

            return values;
        }
    }
}
