using Crestron.SimplSharp;
using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.DeviceSupport;
using LinkLynx.Core.Collections;
using LinkLynx.Core.Collections.Pools;
using LinkLynx.Core.Engine;
using LinkLynx.Core.Utility.Dispatchers;
using LinkLynx.Core.Utility.Registries;
using LinkLynx.Core.Utility.Signals;

namespace LinkLynx.Core
{
    /// <summary>
    /// Public entry point for the LinkLynx framework.
    /// Provides a minimal API to initialize the framework, register panels, and
    /// run page-level initialization logic.
    /// </summary>
    /// <remarks>
    /// Call <see cref="Initialize"/> once at program startup, then register each panel
    /// via <see cref="RegisterPanel"/> before using it. Finally, call
    /// <see cref="InitializePanel"/> to set each panel to its default UI state.
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
        void Initialize();

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
        void RegisterPanel(BasicTriList panel);

        /// <summary>
        /// Runs each page's <c>Initialize()</c> method for the specified panel,
        /// setting default UI state and feedback.
        /// </summary>
        /// <param name="panel">The panel whose pages should be initialized.</param>
        /// <remarks>
        /// Call after <see cref="RegisterPanel(BasicTriList)"/> and once the panel is online.
        /// </remarks>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">
        /// Thrown if the panel has not been registered.
        /// </exception>
        void InitializePanel(BasicTriList panel);

        /// <summary>
        /// Handles any simple signal given, Maps the signal to a device's logic.
        /// </summary>
        /// <param name="panel">The device that is responsible for the signal.</param>
        /// <param name="args">The signal instance that was created for the change.</param>
        void HandelSimpleSignal(BasicTriList panel, SigEventArgs args);

        /// <summary>
        /// Releases all resources and clears registries.
        /// </summary>
        /// <remarks>
        /// Call this before shutting down the control system to prevent memory leaks.
        /// After cleanup, the framework must be re-initialized before use.
        /// </remarks>
        void Cleanup();
    }

    /// <summary>
    /// Public entry point for the LinkLynx framework.
    /// Provides a minimal API to initialize the framework, register panels, and
    /// run page-level initialization logic.
    /// </summary>
    /// <remarks>
    /// Call <see cref="Initialize"/> once at program startup, then register each panel
    /// via <see cref="RegisterPanel"/> before using it. Finally, call
    /// <see cref="InitializePanel"/> to set each panel to its default UI state.
    /// This type is intended to be used as a single instance per program.
    /// </remarks>
    /// <example>
    /// <code>
    /// var fw = new LinkLynx();
    /// fw.Initialize();
    /// fw.RegisterPanel(xPanelOne);
    /// fw.InitializePanel(xPanelOne);
    /// </code>
    /// </example>
    public sealed class LinkLynx : ILinkLynx
    {
        /// <summary>
        /// Scans loaded assemblies for page classes, registers page factories,
        /// and wires join handlers.
        /// </summary>
        /// <remarks>
        /// Must be called before <see cref="RegisterPanel(BasicTriList)"/>.
        /// Safe to call multiple times; subsequent calls are no-ops.
        /// </remarks>
        public void Initialize()
        {
            PageScanner.Run(); // your reflection scanner that registers pages + joins
        }

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
        public void RegisterPanel(BasicTriList panel)
        {
            LogicGroupPool.RegisterPanel(panel);
            panel.Register();
        }

        /// <summary>
        /// Runs each page's <c>Initialize()</c> method for the specified panel,
        /// setting default UI state and feedback.
        /// </summary>
        /// <param name="panel">The panel whose pages should be initialized.</param>
        /// <remarks>
        /// Call after <see cref="RegisterPanel(BasicTriList)"/> and once the panel is online.
        /// </remarks>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">
        /// Thrown if the panel has not been registered.
        /// </exception>
        public void InitializePanel(BasicTriList panel)
        {
            LogicGroupPool.InitializePanelLogic(panel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="args"></param>
        public void HandelSimpleSignal(BasicTriList panel, SigEventArgs args)
        {
            SignalProcessor.ProcessSignalChange(panel, args);
        }

        /// <summary>
        /// Releases all resources and clears registries.
        /// </summary>
        /// <remarks>
        /// Call this before shutting down the control system to prevent memory leaks.
        /// After cleanup, the framework must be re-initialized before use.
        /// </remarks>
        public void Cleanup()
        {
            // 1. Pools
            CrestronConsole.PrintLine("[LinkLynx] Cleaning Panel Pool...");
            PanelPool.Clear();
            CrestronConsole.PrintLine("[LinkLynx] Cleaning Logic Group Pool...");
            LogicGroupPool.Clear();

            // 2. Registries
            CrestronConsole.PrintLine("[LinkLynx] Cleaning The Page Registry...");
            PageRegistry.Clear();
            CrestronConsole.PrintLine("[LinkLynx] Cleaning The Reverse Page Registry...");
            ReversePageRegistry.Clear();

            // 3. Dispatchers
            CrestronConsole.PrintLine("[LinkLynx] Cleaning The Dispatcher...");
            DispatcherHelper.Clear();

            // 4. Log the cleanup
            CrestronConsole.PrintLine("[LinkLynx] Framework clean Successful! Good Night");
        }
    }
}
