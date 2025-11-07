using Crestron.SimplSharpPro;
using LinkLynx.Core.Logic.Pages;
using LinkLynx.Core.CrestronPOCOs;
using System;

namespace LinkLynx.Core.Interfaces.Utility.Dispatching
{
    public interface IJoinDispatcher
    {
        bool AddToDispatcher(Enum join, Action<PageLogicBase, SignalEventData> action);

        bool CheckIfDispatcherContainsKey(Signals.SigType signalType, uint joinId);

        Action<PageLogicBase, SignalEventData> GetDispatcherActionFromKey(Signals.SigType signalType, uint joinId);
    }
}
