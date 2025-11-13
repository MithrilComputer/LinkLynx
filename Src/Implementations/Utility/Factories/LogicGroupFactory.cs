using LinkLynx.Core.CrestronPOCOs;
using LinkLynx.Core.Logic.Pages;
using LinkLynx.Implementations.Collections.PanelContexts;

namespace LinkLynx.Core.Src.Implementations.Utility.Factories
{
    /// <summary>
    /// This class is responsible for creating logic groups.
    /// </summary>
    public static class LogicGroupFactory
    {
        /// <summary>
        /// Creates a new PanelLogicGroup.
        /// </summary>
        public static PanelLogicGroup CreateNewLogicGroup(PanelDevice panel, Dictionary<ushort, PageLogicBase> pages)
        {
            return new PanelLogicGroup(panel, pages);
        }
    }
}
