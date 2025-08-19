using Crestron.SimplSharp;
using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.DeviceSupport;
using System;
using LinkLynx.Core.Utility.Debugging.Logging;

namespace LinkLynx.Core.Utility.Helpers
{
    /// <summary>
    /// Generic Helpers for the VTProIntegrationTestSimpleSharp application.
    /// </summary>
    public static class SignalHelper
    {
        /// <summary>
        /// Sets a digital join on the panel.
        /// </summary>
        /// <param name="panel"> The panel to set the digital join on.</param>
        /// <param name="digitalJoin"> The digital join to set.</param>
        /// <param name="value"> The value to set the digital join to.</param>
        public static void SetDigitalJoin(BasicTriList panel, uint digitalJoin, bool value)
        {
            if (panel == null)
                throw new NullReferenceException("[GHelpers] Error: SetDigitalJoin Device is null, cannot set join.");

            ConsoleLogger.Log($"[GHelpers] Log: Setting Digital Join '{digitalJoin}' on device '{panel.ID}' as '{value}'");

            panel.BooleanInput[digitalJoin].BoolValue = value;
        }

        /// <summary>
        /// Sets a analog join on the panel.
        /// </summary>
        /// <param name="panel"> The panel to set the analog join on.</param>
        /// <param name="analogJoin"> The analogJoin join to set.</param>
        /// <param name="value"> The value to set the analog join to.</param>
        public static void SetAnalogJoin(BasicTriList panel, uint analogJoin, ushort value)
        {
            if (panel == null)
                throw new NullReferenceException("[GHelpers] SetAnalogJoin: Device is null, cannot set join.");

            ConsoleLogger.Log($"[GHelpers] Log: Setting Analog Join '{analogJoin}' on device '{panel.ID}' as '{value}'");

            panel.UShortInput[analogJoin].UShortValue = value;
        }

        /// <summary>
        /// Sets a serial join on the panel.
        /// </summary>
        /// <param name="panel"> The panel to set the serial join on.</param>
        /// <param name="serialJoin"> The serial join to set.</param>
        /// <param name="value"> The value to set the serial join to.</param>
        public static void SetSerialJoin(BasicTriList panel, uint serialJoin, string value)
        {
            if (panel == null)
                throw new NullReferenceException("[GHelpers] SetSerialJoin: Device is null, cannot set join.");

            ConsoleLogger.Log($"[GHelpers] Log: Setting Serial Join '{serialJoin}' on device '{panel.ID}' as '{value}'");

            panel.StringInput[serialJoin].StringValue = value;
        }

        /// <summary>
        /// Sees if a button is pressed on a rising edge. (Not Smart Object)
        /// </summary>
        /// <param name="args">The signal received</param>
        /// <returns>Bool, If the signal was a rising edge</returns>
        public static bool IsRisingEdge(SigEventArgs args)
        {
            if (args.Sig.Type != eSigType.Bool)
                throw new Exception("[GHelpers] Cant check if non-digital signal is a Rising Edge");

            return args.Sig.BoolValue;
        }

        /// <summary>
        /// Sees if a button is released on a falling edge. (Not Smart Object)
        /// </summary>
        /// <param name="args">The signal received</param>
        /// <returns>Bool, If the signal was a falling edge</returns>
        public static bool IsFallingEdge(SigEventArgs args)
        {
            if (args.Sig.Type != eSigType.Bool)
                throw new Exception("[GHelpers] Cant check if non-digital signal is a Rising Edge");

            return !args.Sig.BoolValue;
        }
    }
}
