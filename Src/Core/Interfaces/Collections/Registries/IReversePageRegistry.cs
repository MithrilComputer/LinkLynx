using Crestron.SimplSharpPro;
using System;

namespace LinkLynx.Core.Interfaces
{
    /// <summary>
    /// The interface for Reverse page registry, used for finding a Page ID from a join number.
    /// </summary>
    public interface IReversePageRegistry
    {
        /// <summary>
        /// Looks up and returns the page associated with the Logic Join.
        /// </summary>
        ushort Get(uint join, eSigType type);

        /// <summary>
        /// Registers a Logic Join to a page.
        /// </summary>
        bool TryRegister(Enum join, ushort pageId);

        /// <summary>
        /// Clears the registries
        /// </summary>
        void Clear();
    }
}
