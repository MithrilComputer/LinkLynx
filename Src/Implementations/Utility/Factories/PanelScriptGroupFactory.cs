using LinkLynx.Core.CrestronPOCOs;
using LinkLynx.Core.Logic.Pages;
using LinkLynx.Implementations.Collections.PanelContexts;

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
