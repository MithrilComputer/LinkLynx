using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.DeviceSupport;
using LinkLynx.Core.Interfaces.Collections.Pools;
using LinkLynx.Core.Interfaces.Utility.Debugging.Logging;
using LinkLynx.Core.Src.Core.Interfaces.Wiring.Engine;
using LinkLynx.Implementations.Utility.Debugging.Logging;
using LinkLynx.Implementations.Utility.Dispatching;
using LinkLynx.PublicAPI.Interfaces;
using LinkLynx.Wiring.DI;
using LinkLynx.Wiring.Engine;
using System;

namespace LinkLynx.PublicAPI.Implementations
{
    public sealed class LinkLynx : ILinkLynx
    {
        private readonly ILogger consoleLogger;
        private readonly IAutoRegisterScanner autoRegisterScanner;
        private readonly ILogicGroupPool logicGroupPool;

        private readonly ISignal

        private readonly ServiceProvider serviceProvider;

        public LinkLynx(ServiceProvider serviceProvider, ILogger consoleLogger, IAutoRegisterScanner autoRegisterScanner, ILogicGroupPool logicGroupPool, ) 
        {
            this.consoleLogger = consoleLogger;
            this.autoRegisterScanner = autoRegisterScanner;
            this.logicGroupPool = logicGroupPool;
        }

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
            consoleLogger.Log("[LinkLynx] Initializing Framework...");
            SendSplash();
            autoRegisterScanner.Run(); // your reflection scanner that registers pages + joins
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
            logicGroupPool.RegisterPanel(panel);
            logicGroupPool.InitializePanelLogic(panel);
        }

        /// <summary>
        /// Resets the specified panel to its visually default state.
        /// </summary>
        /// <param name="panel">The <see cref="BasicTriList"/> instance representing the panel to reset. Cannot be <see langword="null"/>.</param>
        public void SetPanelToDefaultState(BasicTriList panel)
        {
            logicGroupPool.SetPanelDefaults(panel);
        }

        /// <summary>
        /// Handles any simple signal given, Maps the signal to a device's logic.
        /// </summary>
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
            serviceProvider.Dispose();

            consoleLogger.Log("[LinkLynx] Framework clean Successful! Good Night...");
        }

        /// <summary>
        /// Running this method prints a Splash Screen To the console.
        /// </summary>
        public void SendSplash()
        {
            consoleLogger.Log(" ");
            consoleLogger.Log($"LinkLynx by MithrilComputers v0.3.0");
            consoleLogger.Log(" ");
            consoleLogger.Log("----------- Happy Hacking! -----------");
            consoleLogger.Log(" ");
        }
    }
}
