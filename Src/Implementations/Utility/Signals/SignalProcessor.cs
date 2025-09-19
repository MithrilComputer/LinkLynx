using LinkLynx.Core.Utility.Debugging.Logging;
using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.DeviceSupport;
using LinkLynx.Core.Collections;
using LinkLynx.Core.Utility.Dispatchers;

namespace LinkLynx.Core.Utility.Signals
{
    /// <summary>
    /// A class that processes signal changes from touch panels.
    /// </summary>
    internal static class SignalProcessor
    {
        /// <summary>
        /// Processes any signal changes that occur from touchPanels or other interactive devices.
        /// </summary>
        /// <param name="device">The Device that called the signal change.</param>
        /// <param name="args">The signal that changed.</param> 
        internal static void ProcessSignalChange(BasicTriList device, SigEventArgs args)
        {
            // Check if the device and signal is null to avoid null reference exceptions.
            if (device == null || args == null)
            {
                ConsoleLogger.Log("[Signal Processor] Device or signal is null, cannot process signal change.");
                return;
            }

            PanelLogicGroup logicGroup = LinkLynxServices.logicGroupPool.GetPanelLogicGroup(device);

            if(logicGroup == null)
            {
                ConsoleLogger.Log($"[Signal Processor] No logic group found for device: {device.Name}");
                return;
            }

            if (device == null || args == null)
            {
                ConsoleLogger.Log("[SignalProcessor] Error: Device or signal is null.");
                return;
            }

            ConsoleLogger.Log("[SignalProcessor] Signal Clear, routing...");

            JoinInstanceRouter.Route(device, args);
        }
    }
}
