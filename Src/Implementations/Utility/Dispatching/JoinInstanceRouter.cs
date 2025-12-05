using LinkLynx.Core.CrestronPOCOs;
using LinkLynx.Core.Interfaces.Collections.Pools;
using LinkLynx.Core.Interfaces.Collections.Registries;
using LinkLynx.Core.Interfaces.Utility.Debugging.Logging;
using LinkLynx.Core.Interfaces.Utility.Dispatching;
using LinkLynx.Core.Logic.Pages;
using LinkLynx.Core.Signals;
using LinkLynx.Implementations.Collections.PanelContexts;
using System;

namespace LinkLynx.Implementations.Utility.Dispatching
{
    /// <summary>
    /// This is a router that ensures that the right device logic is called for a given panel signal.
    /// </summary>
    public class JoinInstanceRouter : IJoinInstanceRouter
    {
        private readonly ILogger consoleLogger;
        private readonly IPanelScriptGroupPool logicGroupPool;
        private readonly IReversePageRegistry reversePageRegistry;
        private readonly IJoinDispatcher dispatcherHelper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="consoleLogger"></param>
        /// <param name="logicGroupPool"></param>
        /// <param name="reversePageRegistry"></param>
        /// <param name="dispatcherHelper"></param>
        public JoinInstanceRouter(ILogger consoleLogger, IPanelScriptGroupPool logicGroupPool, IReversePageRegistry reversePageRegistry, IJoinDispatcher dispatcherHelper) 
        { 
            this.consoleLogger = consoleLogger;
            this.logicGroupPool = logicGroupPool;
            this.reversePageRegistry = reversePageRegistry;
            this.dispatcherHelper = dispatcherHelper;
        }

        /// <summary>
        /// This attempts to run the method given to the correct device logic.
        /// </summary>
        /// <param name="panel">The device to run the method on.</param>
        /// <param name="data">The signal to be processed.</param>
        public void Route(TouchPanelDevice panel, SignalEventData data)
        {
            try
            {
                if (panel == null || data == null)
                {
                    consoleLogger.Log("[Signal Processor] Device or signal is null, cannot process signal change.");
                    return;
                }

                PanelScriptGroup logicGroup = logicGroupPool.GetPanelLogicGroup(panel);

                if (logicGroup == null)
                {
                    consoleLogger.Log($"[Signal Processor] No logic group found for device: {panel.Name}");
                    return;
                }

                if (panel == null || data == null)
                {
                    consoleLogger.Log("[SignalProcessor] Error: Device or signal is null.");
                    return;
                }

                consoleLogger.Log($"[JoinInstanceRouter] Device {panel.IPID}, Sig {data.SignalType.ToString()} #{data.SignalJoinID}");

                PanelScriptGroup group = logicGroupPool.GetPanelLogicGroup(panel);

                ushort pageId = reversePageRegistry.Get(data.SignalJoinID, data.SignalType);

                consoleLogger.Log($"[JoinInstanceRouter] Resolved PageId={pageId}");

                PageLogicScript page = group.GetPageLogicFromId(pageId);

                if (page == null)
                {
                    consoleLogger.Log($"[JoinInstanceRouter] Error: Page {pageId} not found in panel group for device '{panel.IPID}'");
                    return;
                }

                Action<PageLogicScript, SignalEventData> action = dispatcherHelper.GetDispatcherActionFromKey(data.SignalType, data.SignalJoinID);

                if (action == null)
                {
                    consoleLogger.Log($"[JoinInstanceRouter] Warning: No action registered for join {data.SignalJoinID} ({data.SignalType})");
                    return;
                }

                action.Invoke(page, data);
            }
            catch (Exception ex)
            {
                consoleLogger.Log($"[JoinInstanceRouter] Route error: {ex.GetType().Name}: {ex.Message}");
                throw;
            }
        }
    }
}
