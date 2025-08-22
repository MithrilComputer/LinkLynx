using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.DeviceSupport;
using LinkLynx.Core.Collections;
using LinkLynx.Core.Logic.Pages;
using LinkLynx.Core.Utility.Debugging.Logging;
using System;

namespace LinkLynx.Core.Utility.Dispatchers
{
    /// <summary>
    /// This is a router that ensures that the right device logic is called for a given panel signal.
    /// </summary>
    internal static class JoinInstanceRouter
    {
        /// <summary>
        /// This attempts to run the method given to the correct device logic.
        /// </summary>
        /// <param name="device">The device to run the method on.</param>
        /// <param name="args">The signal to be processed.</param>
        internal static void Route(BasicTriList device, SigEventArgs args)
        {
            ConsoleLogger.Log($"[JoinInstanceRouter] Attempting signal route...");

            PanelLogicGroup group = LinkLynxServices.logicGroupPool.GetPanelLogicGroup(device);

            uint join = args.Sig.Number;
            eSigType type = args.Sig.Type;

            ushort pageId = LinkLynxServices.reversePageRegistry.GetPageFromSignalAndType(join, type);

            PageLogicBase page = group.GetPageLogicFromId(pageId);

            if (page == null)
            {
                ConsoleLogger.Log($"[JoinInstanceRouter] Error: Page {pageId} not found in panel group for device '{device.ID}'");
                return;
            }

            Action<PageLogicBase, SigEventArgs> action = DispatcherHelper.GetDispatcherActionFromKey(type, join);

            if (action == null)
            {
                ConsoleLogger.Log($"[JoinInstanceRouter] Warning: No action registered for join {join} ({type})");
                return;
            }

            action.Invoke(page, args);
        }
    }
}
