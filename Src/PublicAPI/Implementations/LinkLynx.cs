using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.DeviceSupport;
using LinkLynx.Core.CrestronPOCOs;
using LinkLynx.Core.Interfaces.Collections.Pools;
using LinkLynx.Core.Interfaces.Utility.Debugging.Logging;
using LinkLynx.Core.Interfaces.Utility.Dispatching;
using LinkLynx.Core.Interfaces.Wiring.Engine;
using LinkLynx.Core.Options;
using LinkLynx.PublicAPI.Interfaces;
using LinkLynx.Wiring.DI;

namespace LinkLynx.PublicAPI.Implementations
{
    /// <summary>
    /// Represents the core framework for managing panels, logic groups, and signal routing in a control system.
    /// </summary>
    /// <remarks>The <see cref="LinkLynx"/> class provides methods for initializing the framework, registering
    /// panels, handling signals, and managing the lifecycle of the control system. It is designed to facilitate the
    /// integration of Crestron panels and their associated logic groups, while offering features such as automatic
    /// registration and resource cleanup.</remarks>
    public sealed class LinkLynx : ILinkLynx
    {
        private readonly string version = "0.0.0";

        private readonly ILogger consoleLogger;

        private readonly IAutoRegisterScanner autoRegisterScanner;

        private readonly ILogicGroupPool logicGroupPool;

        private readonly IJoinInstanceRouter joinInstanceRouter;

        private readonly IPanelPool panelPool;

        private readonly bool autoRegisterPanelsToControlSystem;

        private readonly ServiceProvider serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkLynx"/> class with the specified dependencies and
        /// configuration options.
        /// </summary>
        /// <param name="serviceProvider">The <see cref="ServiceProvider"/> instance used to resolve dependencies.</param>
        /// <param name="consoleLogger">The <see cref="ILogger"/> instance used for logging messages to the console.</param>
        /// <param name="autoRegisterScanner">The <see cref="IAutoRegisterScanner"/> instance responsible for scanning and registering components
        /// automatically.</param>
        /// <param name="logicGroupPool">The <see cref="ILogicGroupPool"/> instance that manages pools of logic groups.</param>
        /// <param name="joinInstanceRouter">The <see cref="IJoinInstanceRouter"/> instance used to route join instances.</param>
        /// <param name="options">The LinkLynx build options <see
        /// langword="true"/> to enable automatic registration; otherwise, <see langword="false"/>.</param>
        /// <param name="panelPool">The <see cref="IPanelPool"/> instance that manages the pool of panels.</param>
        public LinkLynx(ServiceProvider serviceProvider, ILogger consoleLogger, IAutoRegisterScanner autoRegisterScanner, ILogicGroupPool logicGroupPool, IJoinInstanceRouter joinInstanceRouter, LinkLynxBuildOptions options, IPanelPool panelPool)
        {
            this.consoleLogger = consoleLogger;
            this.autoRegisterScanner = autoRegisterScanner;
            this.logicGroupPool = logicGroupPool;
            this.joinInstanceRouter = joinInstanceRouter;

            this.panelPool = panelPool;
            this.serviceProvider = serviceProvider;

            autoRegisterPanelsToControlSystem = options.AutoRegisterPanelsToControlSystem;
        }

        /// <summary>
        /// Scans loaded assemblies for page classes, registers page factories,
        /// and wires join handlers. Not required unless dynamic registration is needed.
        /// </summary>
        /// <remarks>
        /// Safe to call multiple times; subsequent calls are no-ops.
        /// </remarks>
        public ILinkLynx Initialize()
        {
            consoleLogger.Log("[LinkLynx] Initializing Framework...");
            autoRegisterScanner.Run(); // your reflection scanner that registers pages + joins

            SendSplash();

            return this;
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
        /// <exception cref="InvalidOperationException">If a panel fails to register</exception>
        public ILinkLynx RegisterPanel(BasicTriList panel)
        {
            PanelDevice panelDevice = new PanelDevice(panel, consoleLogger);

            if (!panelPool.TryAddPanel(panelDevice.IPID, panelDevice))
                { consoleLogger.Log($"[LinkLynx] Can not register an already registered device. Did you try to register a duplicate?."); return this; }

            logicGroupPool.RegisterPanel(panelDevice);
            logicGroupPool.InitializePanelLogic(panelDevice);

            if(autoRegisterPanelsToControlSystem)
            {
                eDeviceRegistrationUnRegistrationResponse registrationStatus = panel.Register();

                if (registrationStatus != eDeviceRegistrationUnRegistrationResponse.Success)
                {
                    throw new InvalidOperationException($"Failed to register panel, reason: {registrationStatus.ToString()}");
                }
            }

            return this;
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
        public ILinkLynx RegisterPanel(PanelDevice panel)
        {
            if (panel == null)
                throw new ArgumentNullException($"[LinkLynx] Cant register a null panel!");

            if (!panelPool.TryAddPanel(panel.IPID, panel))
            { consoleLogger.Log($"[LinkLynx] Can not register an already registered device. Did you try to register a duplicate?."); return this; }

            logicGroupPool.RegisterPanel(panel);
            logicGroupPool.InitializePanelLogic(panel);

            return this;
        }

        /// <summary>
        /// Resets the specified panel to its visually default state.
        /// </summary>
        /// <param name="panel">The <see cref="BasicTriList"/> instance representing the panel to reset. Cannot be <see langword="null"/>.</param>
        public ILinkLynx SetPanelToDefaultState(BasicTriList panel)
        {
            PanelDevice panelDevice = panelPool.GetPanel(panel.ID);

            logicGroupPool.SetPanelDefaults(panelDevice);

            return this;
        }

        /// <summary>
        /// Resets the specified panel to its visually default state.
        /// </summary>
        /// <param name="panel">The <see cref="BasicTriList"/> instance representing the panel to reset. Cannot be <see langword="null"/>.</param>
        public ILinkLynx SetPanelToDefaultState(PanelDevice panel)
        {
            PanelDevice panelDevice = panelPool.GetPanel(panel.IPID);

            logicGroupPool.SetPanelDefaults(panelDevice);

            return this;
        }

        /// <summary>
        /// Resets the specified panel to its visually default state.
        /// </summary>
        public ILinkLynx SetPanelToDefaultState(uint panelIPID)
        {
            PanelDevice panelDevice = panelPool.GetPanel(panelIPID);

            logicGroupPool.SetPanelDefaults(panelDevice);

            return this;
        }

        /// <summary>
        /// Handles any simple signal given, Maps the signal to a device's logic.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="args"/> is <c>null</c>.</exception>
        public void HandleSimpleSignal(BasicTriList panel, SigEventArgs args)
        {
            if (panel == null) throw new ArgumentNullException(nameof(panel) ,$"[LinkLynx] The Panel Given For The Signal Handling Is Null!");
            if (args == null) throw new ArgumentNullException(nameof(panel), $"[LinkLynx] The Signal Given For The Signal Handling Is Null!");

            // Wrap the Crestron types as my own, avoids issues with testing. Should prob use a factory at some point.
            SignalEventData signalData = new SignalEventData(args);
            PanelDevice panelDevice = panelPool.GetPanel(panel.ID);

            joinInstanceRouter.Route(panelDevice, signalData);
        }

        /// <summary>
        /// Handles any simple signal given, Maps the signal to a device's logic.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="args"/> is <c>null</c>.</exception>
        public void HandleSimpleSignal(PanelDevice panel, SignalEventData args)
        {
            if (panel == null) throw new ArgumentNullException(nameof(panel), $"[LinkLynx] The Panel Given For The Signal Handling Is Null!");
            if (args == null) throw new ArgumentNullException(nameof(panel), $"[LinkLynx] The Signal Given For The Signal Handling Is Null!");

            joinInstanceRouter.Route(panelPool.GetPanel(panel.IPID), args);
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
            consoleLogger.Log($"LinkLynx by MithrilComputers {version}");
            consoleLogger.Log(" ");
            consoleLogger.Log("----------- Happy Hacking! -----------");
            consoleLogger.Log(" ");
        }
    }
}
