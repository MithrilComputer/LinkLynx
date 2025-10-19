using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.DeviceSupport;
using LinkLynx.Core.Src.Core.CrestronPOCOs;
using System;

namespace LinkLynx.Core.Interfaces.Utility.Helpers
{
    internal interface ISignalHelper
    {
        void SetLogicJoin<T>(BasicTriList panel, Enum join, T value);
        bool IsRisingEdge(SignalEventData args);
        bool IsFallingEdge(SignalEventData args);

    }
}
