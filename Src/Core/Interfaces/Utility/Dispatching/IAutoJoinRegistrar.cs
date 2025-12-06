using LinkLynx.Implementations.Collections.Pages.Logic;

namespace LinkLynx.Core.Interfaces.Utility.Dispatching
{
    /// <summary>
    /// The IAutoJoinRegistrar interface defines a contract for automatically registering joins in PageLogic classes to specific page IDs.
    /// </summary>
    public interface IAutoJoinRegistrar
    {
        /// <summary>
        /// Registers all joins in the specified PageLogic class to the provided page ID.
        /// </summary>
        void RegisterJoins<T>(ushort pageId) where T : PageLogicScript;
    }
}
