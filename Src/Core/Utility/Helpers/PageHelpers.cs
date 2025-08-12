using Crestron.SimplSharp;
using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.DeviceSupport;
using System;

namespace LinkLynx.Core.Utility.Registries
{
    /// <summary>
    /// Generic Helpers for the VTProIntegrationTestSimpleSharp application.
    /// </summary>
    public static class PageHelpers
    {
        /// <summary>
        /// Sets a digital join on the device.
        /// </summary>
        /// <param name="device"> The device to set the digital join on.</param>
        /// <param name="digitalJoin"> The digital join to set.</param>
        /// <param name="value"> The value to set the digital join to.</param>
        public static void SetDigitalJoin(BasicTriList device, uint digitalJoin, bool value)
        {
            if (device == null)
                throw new NullReferenceException("[GHelpers] Error: SetDigitalJoin Device is null, cannot set join.");

            CrestronConsole.PrintLine($"[GHelpers] Log: Setting Digital Join '{digitalJoin}' on device '{device.ID}' as '{value}'");

            device.BooleanInput[digitalJoin].BoolValue = value;
        }

        /// <summary>
        /// Sets a analog join on the device.
        /// </summary>
        /// <param name="device"> The device to set the analogJoin join on.</param>
        /// <param name="analogJoin"> The analogJoin join to set.</param>
        /// <param name="value"> The value to set the analogJoin join to.</param>
        public static void SetAnalogJoin(BasicTriList device, uint analogJoin, ushort value)
        {
            if (device == null)
                throw new NullReferenceException("[GHelpers] SetAnalogJoin: Device is null, cannot set join.");

            CrestronConsole.PrintLine($"[GHelpers] Log: Setting Analog Join '{analogJoin}' on device '{device.ID}' as '{value}'");

            device.UShortInput[analogJoin].UShortValue = value;
        }

        /// <summary>
        /// Sets a serial join on the device.
        /// </summary>
        /// <param name="device"> The device to set the serial join on.</param>
        /// <param name="serialJoin"> The serial join to set.</param>
        /// <param name="value"> The value to set the serial join to.</param>
        public static void SetSerialJoin(BasicTriList device, uint serialJoin, string value)
        {
            if (device == null)
                throw new NullReferenceException("[GHelpers] SetSerialJoin: Device is null, cannot set join.");

            CrestronConsole.PrintLine($"[GHelpers] Log: Setting Serial Join '{serialJoin}' on device '{device.ID}' as '{value}'");

            device.StringInput[serialJoin].StringValue = value;
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
    }
}
