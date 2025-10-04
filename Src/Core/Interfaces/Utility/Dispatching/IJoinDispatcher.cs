using Crestron.SimplSharpPro;
using LinkLynx.Core.Logic.Pages;
using System;

namespace LinkLynx.Core.Interfaces.Utility.Dispatching
{
    internal interface IJoinDispatcher
    {
        bool AddToDispatcher(Enum join, Action<PageLogicBase, SigEventArgs> action);

        bool CheckIfDispatcherContainsKey(eSigType signalType, uint joinId);

        Action<PageLogicBase, SigEventArgs> GetDispatcherActionFromKey(eSigType signalType, uint joinId);
    }
}
