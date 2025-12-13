using LinkLynx.Core.CrestronWrappers;
using LinkLynx.Implementations.Collections.Pages.Contexts;
using LinkLynx.Implementations.Collections.Pages.Logic;

namespace LinkLynx.Core.Src.Core.Interfaces.Utility.Factories
{
    /// <summary>
    /// This interface is responsible for creating logic groups.
    /// </summary>
    public interface IPanelScriptGroupFactory
    {
        /// <summary>
        /// Creates a new PanelLogicGroup.
        /// </summary>
        PanelScriptGroup CreateNewLogicGroup(TouchPanelDevice panel, Dictionary<ushort, PageLogicScript> pages);
    }
}
