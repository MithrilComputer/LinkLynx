using LinkLynx.Core.CrestronWrappers;
using LinkLynx.Core.Interfaces.Utility.Debugging.Logging;
using LinkLynx.Core.Src.Core.Interfaces.Utility.Factories;
using LinkLynx.Implementations.Collections.Pages.Contexts;
using LinkLynx.Implementations.Collections.Pages.Logic;

namespace LinkLynx.Core.Src.Implementations.Utility.Factories
{
    /// <summary>
    /// This class is responsible for creating logic groups.
    /// </summary>
    public class PanelScriptGroupFactory : IPanelScriptGroupFactory
    {
        private readonly ILogger logger;

        /// <summary>
        /// Constructor for PanelScriptGroupFactory.
        /// </summary>
        public PanelScriptGroupFactory(ILogger logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Creates a new PanelLogicGroup.
        /// </summary>
        public PanelScriptGroup CreateNewLogicGroup(TouchPanelDevice panel, Dictionary<ushort, PageLogicScript> pages)
        {
            return new PanelScriptGroup(panel, pages, logger);
        }
    }
}
