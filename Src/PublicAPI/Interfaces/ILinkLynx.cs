using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.DeviceSupport;
using LinkLynx.Core.CrestronPOCOs;

namespace LinkLynx.PublicAPI.Interfaces
{
    /// <summary>
    /// Public entry point for the LinkLynx framework.
    /// Provides a minimal API to initialize the framework, register panels, and
    /// run page-level initialization logic.
    /// </summary>
    /// <remarks>
    /// Call <see cref="Initialize"/> once at program startup, then register each panel
    /// via <see cref="RegisterPanel"/> before using it. Finally, call
    /// <see cref="SetPanelToDefaultState"/> to set a panel to its default UI state whenever needed.
    /// This type is intended to be used as a single instance per program.
    /// </remarks>
    /// <example>
    /// <code>
    /// LinkLynx linkLynx = new LinkLynx();
    /// 
    /// linkLynx.Initialize();
    /// linkLynx.RegisterPanel(xPanelOne);
    /// linkLynx.InitializePanel(xPanelOne);
    /// </code>
    /// </example>
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
        ILinkLynx RegisterPanel(PanelDevice panel);

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
        ILinkLynx SetPanelToDefaultState(PanelDevice panel);

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
        void HandleSimpleSignal(PanelDevice panel, SignalEventData args);

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
