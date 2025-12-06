using LinkLynx.Core.CrestronWrappers;
using LinkLynx.Implementations.Collections.Pages.Contexts;
using LinkLynx.Implementations.Collections.Pages.Logic;

namespace LinkLynx.Core.Src.Implementations.Utility.Factories
{
    /// <summary>
    /// This class is responsible for creating logic groups.
    /// </summary>
    public static class PanelScriptGroupFactory
    {
        // TODO Take a look at this factory later.

        /// <summary>
        /// Creates a new PanelLogicGroup.
        /// </summary>
        public static PanelScriptGroup CreateNewLogicGroup(TouchPanelDevice panel, Dictionary<ushort, PageLogicScript> pages)
        {
            return new PanelScriptGroup(panel, pages);
        }
    }
}
