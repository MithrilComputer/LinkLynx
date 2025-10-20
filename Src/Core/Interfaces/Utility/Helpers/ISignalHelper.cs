using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.DeviceSupport;
using LinkLynx.Core.CrestronPOCOs;
using System;

namespace LinkLynx.Core.Interfaces.Utility.Helpers
{
    internal interface ISignalHelper
    {
        void SetLogicJoin<T>(PanelDevice panel, Enum join, T value);
        bool IsRisingEdge(SignalEventData args);
        bool IsRisingEdge(SigEventArgs args);
    }
}
