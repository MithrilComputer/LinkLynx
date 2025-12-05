using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.DeviceSupport;
using LinkLynx.Core.CrestronPOCOs;

namespace LinkLynx.PublicAPI.Interfaces
{
    /// <summary>
    /// Defines the contract for managing and interacting with Crestron panels, including initialization, panel
    /// registration, signal handling, and resource cleanup.
    /// </summary>
    /// <remarks>This interface provides methods to initialize the framework, register panels, handle signals,
    /// reset panels to their default state, and release resources. It is designed to support Crestron control systems
    /// and ensures proper lifecycle management of panel logic and resources.</remarks>
    public interface ILinkLynx
    {
        /// <summary>
        /// Scans loaded assemblies for page classes, registers page factories,
        /// and wires join handlers.
        /// </summary>
        /// <remarks>
        /// Must be called before <see cref="RegisterPanel(BasicTriList)"/>.
        /// Safe to call multiple times; subsequent calls are no-ops.
        /// </remarks>
        ILinkLynx Initialize();

        /// <summary>
        /// Registers a panel with the framework and creates its page logic group.
        /// </summary>
        /// <param name="panel">The Crestron panel to register.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown if <paramref name="panel"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// Thrown if the panel is already registered.
        /// </exception>
        ILinkLynx RegisterPanel(BasicTriList panel);

        /// <summary>
        /// Registers a panel with the framework and creates its page logic group.
        /// </summary>
        /// <param name="panel">The Crestron panel to register.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown if <paramref name="panel"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// Thrown if the panel is already registered.
        /// </exception>
        ILinkLynx RegisterPanel(TouchPanelDevice panel);

        /// <summary>
        /// Resets the specified panel to its visually default state.
        /// </summary>
        /// <param name="panel">The <see cref="BasicTriList"/> instance representing the panel to reset. Cannot be <see langword="null"/>.</param>
        ILinkLynx SetPanelToDefaultState(BasicTriList panel);

        /// <summary>
        /// Resets the specified panel to its visually default state.
        /// </summary>
        ILinkLynx SetPanelToDefaultState(uint panelIPID);

        /// <summary>
        /// Resets the specified panel to its visually default state.
        /// </summary>
        /// <param name="panel">The <see cref="BasicTriList"/> instance representing the panel to reset. Cannot be <see langword="null"/>.</param>
        ILinkLynx SetPanelToDefaultState(TouchPanelDevice panel);

        /// <summary>
        /// Handles any simple signal given, Maps the signal to a device's logic.
        /// </summary>
        /// <param name="panel">The device that is responsible for the signal.</param>
        /// <param name="args">The signal instance that was created for the change.</param>
        void HandleSimpleSignal(BasicTriList panel, SigEventArgs args);

        /// <summary>
        /// Handles any simple signal given, Maps the signal to a device's logic.
        /// </summary>
        /// <param name="panel">The device that is responsible for the signal.</param>
        /// <param name="args">The signal instance that was created for the change.</param>
        void HandleSimpleSignal(TouchPanelDevice panel, SignalEventData args);

        /// <summary>
        /// Releases all resources and clears registries.
        /// </summary>
        /// <remarks>
        /// Call this before shutting down the control system to prevent memory leaks.
        /// After cleanup, the framework must be re-initialized before use.
        /// </remarks>
        void Cleanup();
    }
}
