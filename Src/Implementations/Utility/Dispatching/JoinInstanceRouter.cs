using LinkLynx.Core.CrestronWrappers;
using LinkLynx.Core.Interfaces.Collections.Dispatchers;
using LinkLynx.Core.Interfaces.Collections.Pools;
using LinkLynx.Core.Interfaces.Collections.Registries;
using LinkLynx.Core.Interfaces.Utility.Debugging.Logging;
using LinkLynx.Core.Interfaces.Utility.Dispatching;
using LinkLynx.Core.Src.Core.Interfaces.Collections.Registries;
using LinkLynx.Implementations.Collections.Pages.Contexts;
using LinkLynx.Implementations.Collections.Pages.Logic;

namespace LinkLynx.Implementations.Utility.Dispatching
{
    /// <summary>
    /// This is a router that ensures that the right device logic is called for a given panel signal.
    /// </summary>
    public class JoinInstanceRouter : IJoinInstanceRouter
    {
        private readonly ILogger consoleLogger;
        private readonly IPanelScriptGroupPool logicGroupPool;
        private readonly ISimpleReversePanelScriptRegistry reversePanelScriptRegistry;
        private readonly IJoinDispatcher dispatcherHelper;
        private readonly IContractIDNameRegistry contractIDNameRegistry;
        private readonly IContractActionDispatcher contractActionDispatcher;

        /// <summary>
        /// The constructor for the Join Instance Router.
        /// </summary>
        public JoinInstanceRouter(ILogger consoleLogger, IPanelScriptGroupPool logicGroupPool, ISimpleReversePanelScriptRegistry reversePageRegistry, IJoinDispatcher dispatcherHelper, IContractIDNameRegistry contractIDNameRegistry, IContractActionDispatcher contractActionDispatcher)
        {
            this.consoleLogger = consoleLogger;
            this.logicGroupPool = logicGroupPool;
            this.reversePanelScriptRegistry = reversePageRegistry;
            this.dispatcherHelper = dispatcherHelper;
            this.contractIDNameRegistry = contractIDNameRegistry;
            this.contractActionDispatcher = contractActionDispatcher;
        }

        /// <summary>
        /// This attempts to run the method given to the correct device logic.
        /// </summary>
        /// <param name="panel">The device to run the method on.</param>
        /// <param name="signalData">The signal to be processed.</param>
        public void Route(TouchPanelDevice panel, SignalEventData signalData)
        {
            try
            {
                if (panel == null || signalData == null)
                {
                    consoleLogger.Log("[Signal Processor] Device or signal is null, cannot process signal change.");
                    return;
                }

                PanelScriptGroup logicGroup = panel.ScriptGroup;

                if (logicGroup == null)
                {
                    consoleLogger.Log($"[Signal Processor] No logic group found for device: {panel.Name}");
                    return;
                }

                if (panel == null || signalData == null)
                {
                    consoleLogger.Log("[SignalProcessor] Error: Device or signal is null.");
                    return;
                }

                consoleLogger.Log($"[JoinInstanceRouter] Device {panel.IPID}, Sig {signalData.SignalType.ToString()} #{signalData.SignalJoinID}");

                PanelScriptGroup group = logicGroupPool.GetPanelLogicGroup(panel);

                if(!reversePanelScriptRegistry.TryGet(signalData.SignalJoinID, signalData.SignalType, out ushort scriptID))
                {
                    consoleLogger.Log($"[JoinInstanceRouter] Warning: No page found for join {signalData.SignalJoinID} ({signalData.SignalType}) on device '{panel.IPID}'");

                    return;
                }

                consoleLogger.Log($"[JoinInstanceRouter] Resolved PageId={scriptID}");

                PageLogicScript page = group.GetScriptLogicFromId(scriptID);

                if (page == null)
                {
                    consoleLogger.Log($"[JoinInstanceRouter] Error: Page {scriptID} not found in panel group for device '{panel.IPID}'");
                    return;
                }

                Action<PageLogicScript, SignalEventData> action;

                // This first checks to see if the join is part of a contract, and if so uses the contract action dispatcher to get the action.
                if (contractIDNameRegistry.TryGet(
                    signalData.SignalJoinID,
                    Core.Signals.SignalDirection.Incoming,
                    out string contractName))
                {
                    action = contractActionDispatcher.Get(contractName);
                }
                else
                {
                    action = dispatcherHelper.GetDispatcherActionFromKey(signalData.SignalType, signalData.SignalJoinID);
                }

                if (action == null)
                {
                    consoleLogger.Log($"[JoinInstanceRouter] Warning: No action registered for join {signalData.SignalJoinID} ({signalData.SignalType})");
                    return;
                }

                action.Invoke(page, signalData);
            }
            catch (Exception ex)
            {
                consoleLogger.Log($"[JoinInstanceRouter] Route error: {ex.GetType().Name}: {ex.Message}");
                throw;
            }
        }
    }
}
