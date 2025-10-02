using Crestron.SimplSharpPro;
using LinkLynx.Core.Logic.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkLynx.Core.Interfaces.Utility.Dispatching
{
    internal interface IJoinDispatcher
    {
        bool AddToDispatcher(Enum join, Action<PageLogicBase, SigEventArgs> action);

        bool CheckIfDispatcherContainsKey(eSigType signalType, uint joinId);

        Action<PageLogicBase, SigEventArgs> GetDispatcherActionFromKey(eSigType signalType, uint joinId);
    }
}
