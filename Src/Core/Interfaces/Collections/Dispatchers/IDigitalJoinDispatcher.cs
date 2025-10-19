using Crestron.SimplSharpPro;
using LinkLynx.Core.Logic.Pages;
using LinkLynx.Core.CrestronPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkLynx.Core.Interfaces.Collections.Dispatchers
{
    internal interface IDigitalJoinDispatcher
    {
        int Count { get; }

        bool TryAdd(uint joinId, Action<PageLogicBase, SignalEventData> action);   

        bool Contains(uint joinId);

        Action<PageLogicBase, SignalEventData> Get(uint joinId);
    }
}
