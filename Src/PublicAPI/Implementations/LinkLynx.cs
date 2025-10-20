using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.DeviceSupport;
using LinkLynx.Core.CrestronPOCOs;
using LinkLynx.Core.Interfaces.Collections.Pools;
using LinkLynx.Core.Interfaces.Utility.Debugging.Logging;
using LinkLynx.Core.Src.Core.Interfaces.Utility.Dispatching;
using LinkLynx.Core.Src.Core.Interfaces.Wiring.Engine;
using LinkLynx.Implementations.Collections.Pools;
using LinkLynx.PublicAPI.Interfaces;
using LinkLynx.Wiring.DI;
using System;

namespace LinkLynx.PublicAPI.Implementations
{
    public sealed class LinkLynx : ILinkLynx
    {
        private string version = "0.0.0";

        private readonly ILogger consoleLogger;
        private readonly IAutoRegisterScanner autoRegisterScanner;
        private readonly ILogicGroupPool logicGroupPool;

        private readonly IJoinInstanceRouter joinInstanceRouter;

        private readonly IPanelPool panelPool;

        private bool autoRegisterPanelsToControlSystem;

        private readonly ServiceProvider serviceProvider;

        public LinkLynx(ServiceProvider serviceProvider, ILogger consoleLogger, IAutoRegisterScanner autoRegisterScanner, ILogicGroupPool logicGroupPool, IJoinInstanceRouter joinInstanceRouter, bool autoRegisterPanelsToControlSystem, IPanelPool panelPool) 
        {
            this.consoleLogger = consoleLogger;
            this.autoRegisterScanner = autoRegisterScanner;
            this.logicGroupPool = logicGroupPool;
            this.joinInstanceRouter = joinInstanceRouter;

            this.autoRegisterPanelsToControlSystem = autoRegisterPanelsToControlSystem;

            this.panelPool = panelPool;
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
            autoRegisterScanner.Run(); // your reflection scanner that registers pages + joins

            SendSplash();
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
            PanelDevice panelDevice = new PanelDevice(panel);

            if (!panelPool.TryAddPanel(panelDevice.IPID, panelDevice))
                { consoleLogger.Log($"[LinkLynx] Can not register an already registered device. Did you try to register a duplicate?."); return; }

            logicGroupPool.RegisterPanel(panelDevice);
            logicGroupPool.InitializePanelLogic(panelDevice);

            if(autoRegisterPanelsToControlSystem)
            {
                eDeviceRegistrationUnRegistrationResponse registrationStatus = panel.Register();

                if (registrationStatus != eDeviceRegistrationUnRegistrationResponse.Success)
                {
                    throw new Exception($"Failed to register panel, reason: {registrationStatus.ToString()}");
                }
            }
        }

        /// <summary>
        /// Resets the specified panel to its visually default state.
        /// </summary>
        /// <param name="panel">The <see cref="BasicTriList"/> instance representing the panel to reset. Cannot be <see langword="null"/>.</param>
        public void SetPanelToDefaultState(BasicTriList panel)
        {
            PanelDevice panelDevice = panelPool.GetPanel(panel.ID);

            logicGroupPool.SetPanelDefaults(panelDevice);
        }

        /// <summary>
        /// Resets the specified panel to its visually default state.
        /// </summary>
        public void SetPanelToDefaultState(uint panelIPID)
        {
            PanelDevice panelDevice = panelPool.GetPanel(panelIPID);

            logicGroupPool.SetPanelDefaults(panelDevice);
        }
        
        /// <summary>
        /// Handles any simple signal given, Maps the signal to a device's logic.
        /// </summary>
        public void HandleSimpleSignal(BasicTriList panel, SigEventArgs args)
        {
            // Wrap the Crestron types as my own, avoids issues with testing. Should prob use a factory at some point.
            SignalEventData signalData = new SignalEventData(args);

            PanelDevice panelDevice = panelPool.GetPanel(panel.ID); 

            joinInstanceRouter.Route(panelDevice, signalData);
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
