
using LinkLynx.Core.Signals;
using System;

namespace LinkLynx.Core.Interfaces.Collections.Registries
{
    /// <summary>
    /// The interface for Reverse page registry, used for finding a Page ID from a join number.
    /// </summary>
    public interface ISimpleReversePageRegistry
    {
        /// <summary>
        /// Looks up and returns the page associated with the Logic Join.
        /// </summary>
        bool TryGet(uint join, SigType type, out ushort pageID);

        /// <summary>
        /// Registers a Logic Join to a page.
        /// </summary>
        bool TryRegister(Enum join, ushort pageId);
    }
}
