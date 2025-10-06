using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.DeviceSupport;
using System;

namespace LinkLynx.Core.Interfaces.Utility.Helpers
{
    internal interface ISignalHelper
    {
        void SetLogicJoin<T>(BasicTriList panel, Enum join, T value);
        bool IsRisingEdge(SigEventArgs args);
        bool IsFallingEdge(SigEventArgs args);

    }
}
