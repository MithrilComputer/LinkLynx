using Crestron.SimplSharpPro.DeviceSupport;
using LinkLynx.Core.CrestronWrappers;
using LinkLynx.Core.Logic.Pages;
using System;
using System.Collections.Generic;

namespace LinkLynx.Core.Interfaces.Collections.Registries
{
    /// <summary>
    /// Defines a contract for managing the registration and retrieval of page logic factories associated with unique
    /// page identifiers.
    /// </summary>
    /// <remarks>This interface provides methods to register page logic factories, retrieve specific page
    /// logic factories by their unique identifiers, and access all registered factories as a read-only collection. It
    /// is designed to facilitate dynamic creation and resolution of page logic at runtime based on the provided <see
    /// cref="TouchPanelDevice"/> context.</remarks>
    public interface IPageScriptRegistry
    {
        /// <summary>
        /// Registers a page with the specified identifier and logic factory function.
        /// </summary>
        /// <remarks>This method associates a page identifier with a logic factory function, enabling the
        /// creation of page logic dynamically at runtime. If a page with the specified <paramref name="pageId"/> is
        /// already registered, the existing registration will be replaced.</remarks>
        /// <param name="pageId">The unique identifier for the page to be registered. Must be a non-negative value.</param>
        /// <param name="pageLogic">A factory function that takes a <see cref="TouchPanelDevice"/> instance and returns an instance of <see
        /// cref="PageLogicScript"/> representing the logic for the page. This function must not return <see
        /// langword="null"/>.</param>
        void RegisterPage(ushort pageId, Func<TouchPanelDevice, PageLogicScript> pageLogic);

        /// <summary>
        /// Retrieves a function that resolves a page based on the specified page ID.
        /// </summary>
        /// <remarks>The returned function allows dynamic resolution of pages based on the provided <see
        /// cref="TouchPanelDevice"/> context. Ensure that the <paramref name="pageId"/> corresponds to a valid page;
        /// otherwise, the behavior of the function may be undefined.</remarks>
        /// <param name="pageId">The unique identifier of the page to retrieve. Must be a valid page ID.</param>
        /// <returns>A function that takes a <see cref="TouchPanelDevice"/> as input and returns the corresponding <see
        /// cref="PageLogicScript"/> for the specified page ID.</returns>
        Func<TouchPanelDevice, PageLogicScript> GetPage(ushort pageId);

        /// <summary>
        /// Retrieves a read-only dictionary containing all registries, where each registry is represented  by a
        /// key-value pair of a unique identifier and a factory method.
        /// </summary>
        /// <remarks>The returned dictionary provides a mapping of registry identifiers to their
        /// corresponding factory methods,  which can be used to create instances of <see cref="PageLogicScript"/> based
        /// on a given <see cref="TouchPanelDevice"/>.</remarks>
        /// <returns>A read-only dictionary where the key is a <see cref="ushort"/> representing the registry identifier,  and
        /// the value is a <see cref="Func{T, TResult}"/> that takes a <see cref="TouchPanelDevice"/> as input  and returns a
        /// <see cref="PageLogicScript"/> instance.</returns>
        IReadOnlyDictionary<ushort, Func<TouchPanelDevice, PageLogicScript>> GetAllRegistries();
    }
}
