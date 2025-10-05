using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.DeviceSupport;
using LinkLynx.Core.Interfaces.Collections.Pools;
using LinkLynx.Core.Interfaces.Collections.Registries;
using LinkLynx.Core.Interfaces.Utility.Debugging.Logging;
using LinkLynx.Core.Interfaces.Utility.Dispatching;
using LinkLynx.Core.Logic.Pages;
using LinkLynx.Implementations.Collections.PanelContexts;
using System;

namespace LinkLynx.Implementations.Utility.Dispatching
{
    /// <summary>
    /// This is a router that ensures that the right device logic is called for a given panel signal.
    /// </summary>
    internal class JoinInstanceRouter
    {
        private readonly ILogger consoleLogger;
        private readonly ILogicGroupPool logicGroupPool;
        private readonly IReversePageRegistry reversePageRegistry;
        private readonly IJoinDispatcher dispatcherHelper;

        public JoinInstanceRouter(ILogger consoleLogger, ILogicGroupPool logicGroupPool, IReversePageRegistry reversePageRegistry, IJoinDispatcher dispatcherHelper) 
        { 
            this.consoleLogger = consoleLogger;
            this.logicGroupPool = logicGroupPool;
            this.reversePageRegistry = reversePageRegistry;
            this.dispatcherHelper = dispatcherHelper;
        }

        /// <summary>
        /// This attempts to run the method given to the correct device logic.
        /// </summary>
        /// <param name="device">The device to run the method on.</param>
        /// <param name="args">The signal to be processed.</param>
        public void Route(BasicTriList device, SigEventArgs args)
        {
            try
            {
                if (device == null || args == null)
                {
                    consoleLogger.Log("[Signal Processor] Device or signal is null, cannot process signal change.");
                    return;
                }

                PanelLogicGroup logicGroup = logicGroupPool.GetPanelLogicGroup(device);

                if (logicGroup == null)
                {
                    consoleLogger.Log($"[Signal Processor] No logic group found for device: {device.Name}");
                    return;
                }

                if (device == null || args == null)
                {
                    consoleLogger.Log("[SignalProcessor] Error: Device or signal is null.");
                    return;
                }

                uint join = args.Sig.Number;
                Core.Signals.eSigType type = (Core.Signals.eSigType)Enum.Parse(typeof(Core.Signals.eSigType),args.Sig.Type.ToString(),ignoreCase: true);

                consoleLogger.Log($"[JoinInstanceRouter] Device {device.ID}, Sig {type} #{join}, Bool={args.Sig.BoolValue}");

                PanelLogicGroup group = logicGroupPool.GetPanelLogicGroup(device);

                ushort pageId = reversePageRegistry.Get(join, type);

                consoleLogger.Log($"[JoinInstanceRouter] Resolved PageId={pageId}");

                PageLogicBase page = group.GetPageLogicFromId(pageId);

                if (page == null)
                {
                    consoleLogger.Log($"[JoinInstanceRouter] Error: Page {pageId} not found in panel group for device '{device.ID}'");
                    return;
                }

                Action<PageLogicBase, SigEventArgs> action = dispatcherHelper.GetDispatcherActionFromKey(type, join);

                if (action == null)
                {
                    consoleLogger.Log($"[JoinInstanceRouter] Warning: No action registered for join {join} ({type})");
                    return;
                }

                action.Invoke(page, args);
            }
            catch (Exception ex)
            {
                consoleLogger.Log($"[JoinInstanceRouter] Route error: {ex.GetType().Name}: {ex.Message}");
                throw;
            }
        }
    }
}
