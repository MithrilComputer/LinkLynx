using LinkLynx.Core.CrestronWrappers;
using LinkLynx.Core.Logic.Pages;
using System.Collections.Generic;

namespace LinkLynx.Core.Interfaces.Utility.Factories
{
    /// <summary>
    /// The page factory interface defines a contract for building page logic instances for a given panel device.
    /// </summary>
    public interface IPageScriptFactory
    {
        /// <summary>
        /// Builds and returns a dictionary of page logic instances for the specified panel device.
        /// </summary>
        Dictionary<ushort, PageLogicScript> BuildPagesForPanel(TouchPanelDevice panel);
    }
}
