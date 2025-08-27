using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.DeviceSupport;
using LinkLynx.Core.Engine;
using LinkLynx.Core.Utility.Debugging.Logging;
using LinkLynx.Core.Utility.Dispatchers;
using LinkLynx.Core.Utility.Signals;
using System;
using System.Reflection;

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
        void HandleSimpleSignal(BasicTriList panel, SigEventArgs args);

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
            ConsoleLogger.Log("[LinkLynx] Initializing Framework...");
            SendSplash();
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
            LinkLynxServices.logicGroupPool.RegisterPanel(panel);
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
            LinkLynxServices.logicGroupPool.InitializePanelLogic(panel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="args"></param>
        public void HandleSimpleSignal(BasicTriList panel, SigEventArgs args)
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
            ConsoleLogger.Log("[LinkLynx] Cleaning Panel Pool...");
            LinkLynxServices.panelPool.Clear();
            ConsoleLogger.Log("[LinkLynx] Cleaning Logic Group Pool...");
            LinkLynxServices.logicGroupPool.Clear();

            // 2. Registries
            ConsoleLogger.Log("[LinkLynx] Cleaning The Page Registry...");
            LinkLynxServices.pageRegistry.Clear();
            ConsoleLogger.Log("[LinkLynx] Cleaning The Reverse Page Registry...");
            LinkLynxServices.reversePageRegistry.Clear();

            // 3. Dispatchers
            ConsoleLogger.Log("[LinkLynx] Cleaning The Dispatcher...");
            DispatcherHelper.Clear();

            // 4. Log the cleanup
            ConsoleLogger.Log("[LinkLynx] Framework clean Successful! Good Night...");
        }

        /// <summary>
        /// Running this method prints a Splash Screen To the console.
        /// </summary>
        public void SendSplash()
        {
            ConsoleLogger.Log(" ");
            ConsoleLogger.Log($"LinkLynx by MithrilComputers v0.3.0");
            ConsoleLogger.Log(" ");
            ConsoleLogger.Log("----------- Happy Hacking! -----------");
            ConsoleLogger.Log(" ");
        }
    }
}
