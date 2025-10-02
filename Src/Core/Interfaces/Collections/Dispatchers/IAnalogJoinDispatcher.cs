using Crestron.SimplSharpPro;
using LinkLynx.Core.Logic.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkLynx.Core.Interfaces.Collections.Dispatchers
{
    internal interface IAnalogJoinDispatcher
    {
        int Count { get; }

        bool TryAdd(uint joinId, Action<PageLogicBase, SigEventArgs> action);

        bool Contains(uint joinId);

        Action<PageLogicBase, SigEventArgs> Get(uint joinId);
    }
}
