using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.DeviceSupport;
using LinkLynx.Core.CrestronWrappers;
using System;

namespace LinkLynx.Core.Interfaces.Utility.Helpers
{
    /// <summary>
    /// This interface defines helper methods for signal processing and manipulation within Crestron panel devices.
    /// </summary>
    public interface ISignalHelper
    {
        /// <summary>
        /// Sets a logic join on a specified panel device to a given value.
        /// </summary>
        void SetLogicJoin<T>(TouchPanelDevice panel, Enum join, T value);

        /// <summary>
        /// Detects if a signal event represents a rising edge transition.
        /// </summary>
        bool IsRisingEdge(SignalEventData args);

        /// <summary>
        /// Checks if the provided SigEventArgs indicates a rising edge transition.
        /// </summary>
        bool IsRisingEdge(SigEventArgs args);
    }
}
