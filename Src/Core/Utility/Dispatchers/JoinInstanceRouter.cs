using Crestron.SimplSharp;
using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.DeviceSupport;
using LinkLynx.Core.Collections;
using LinkLynx.Core.Logic;
using LinkLynx.Core.Utility.Registries;
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
            PanelLogicGroup group = LogicGroupPool.GetPanelLogicGroup(device);

            if (group == null)
            {
                CrestronConsole.PrintLine($"[JoinInstanceRouter] Error: No logic group found for device '{device.ID}'");
                return;
            }

            try
            {
                uint join = args.Sig.Number;
                eSigType type = args.Sig.Type;

                ushort pageId = ReversePageRegistry.GetPageFromSignalAndType(join, type);

                PageLogicBase page = group.GetPageLogicFromID(pageId);

                if (page == null)
                {
                    CrestronConsole.PrintLine($"[JoinInstanceRouter] Error: Page {pageId} not found in panel group for device '{device.ID}'");
                    return;
                }

                Action<PageLogicBase, SigEventArgs> action = DispatcherHelper.GetDispatcherActionFromKey(type, join);

                if (action == null)
                {
                    CrestronConsole.PrintLine($"[JoinInstanceRouter] Warning: No action registered for join {join} ({type})");
                    return;
                }

                action.Invoke(page, args);
            }
            catch (Exception ex)
            {
                CrestronConsole.PrintLine($"[JoinInstanceRouter] Error: {ex.Message}");
            }
        }
    }
}
