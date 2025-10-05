using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.DeviceSupport;
using LinkLynx.Core.Interfaces.Collections.Registries;
using LinkLynx.Core.Interfaces.Utility.Debugging.Logging;
using System;

namespace LinkLynx.Implementations.Utility.Helpers
{
    /// <summary>
    /// Generic Helpers for the VTProIntegrationTestSimpleSharp application.
    /// </summary>
    public class SignalHelper
    {
        private readonly IEnumSignalTypeRegistry enumSignalTypeRegistry;
        private readonly ILogger consoleLogger;

        /// <summary>
        /// Constructor
        /// </summary>
        public SignalHelper(IEnumSignalTypeRegistry enumSignalTypeRegistry, ILogger consoleLogger)
        {
            this.enumSignalTypeRegistry = enumSignalTypeRegistry;
            this.consoleLogger = consoleLogger;
        }

        /// <summary>
        /// Sets a logic join on a given panel.
        /// </summary>
        /// <param name="panel"> The panel to set the logic join on.</param>
        /// <param name="join"> The join to set.</param>
        /// <param name="value"> The value to set the join to.</param>
        public void SetLogicJoin<T>(BasicTriList panel, Enum join, T value)
        {
            if (panel == null)
                throw new ArgumentNullException($"[SignalHelper] SetSerialJoin: Device is null, cannot set join.");

            Core.Signals.eSigType signalType = enumSignalTypeRegistry.Get(join.GetType());
            
            ushort joinNumber = Convert.ToUInt16(join);

            consoleLogger.Log($"[SignalHelper] Setting Join '{Convert.ToUInt16(join)}' on device '{panel.ID}' as '{value}'");

            switch (signalType)
            {
                case Core.Signals.eSigType.Bool:

                    if (typeof(T) == typeof(bool))
                    {
                        bool trueValue = (bool)(object)value;

                        panel.BooleanInput[joinNumber].BoolValue = trueValue;
                    }
                    else
                        throw new ArgumentException($"[SignalHelper] Error: Type mismatch when attempting to set join for a given Enum: {join}, and Value: {value}");
                    break;

                case Core.Signals.eSigType.String:

                    if (typeof(T) == typeof(string))
                    {
                        string trueValue = (string)(object)value;

                        panel.StringInput[joinNumber].StringValue = trueValue;
                    }
                    else
                        throw new ArgumentException($"[SignalHelper] Error: Type mismatch when attempting to set join for a given Enum: {join}, and Value: {value}");
                    break;

                case Core.Signals.eSigType.UShort:

                    if (typeof(T) == typeof(ushort))
                    {
                        ushort trueValue = (ushort)(object)value;

                        panel.UShortInput[joinNumber].UShortValue = trueValue;
                    }
                    else
                        throw new ArgumentException($"[SignalHelper] Error: Type mismatch when attempting to set join for a given Enum: {join}, and Value: {value}");
                    break;

                case Core.Signals.eSigType.NA:
                        throw new ArgumentException($"[SignalHelper] Error: Cannot process a NA type!");
            }
        }

        /// <summary>
        /// Sees if a button is pressed on a rising edge. (Not Smart Object)
        /// </summary>
        /// <param name="args">The signal received</param>
        /// <returns>Bool, If the signal was a rising edge</returns>
        public static bool IsRisingEdge(SigEventArgs args)
        {
            if (args.Sig.Type != eSigType.Bool)
                throw new Exception("[SignalHelper] Cant check if non-digital signal is a Rising Edge");

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
                throw new Exception("[SignalHelper] Cant check if non-digital signal is a Falling Edge");

            return !args.Sig.BoolValue;
        }
    }
}
