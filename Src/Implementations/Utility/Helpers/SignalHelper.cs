using Crestron.SimplSharpPro;
using LinkLynx.Core.CrestronPOCOs;
using LinkLynx.Core.Interfaces.Collections.Registries;
using LinkLynx.Core.Interfaces.Utility.Debugging.Logging;
using LinkLynx.Core.Interfaces.Utility.Helpers;
using LinkLynx.Core.Signals;

namespace LinkLynx.Implementations.Utility.Helpers
{
    /// <summary>
    /// Generic Helpers for the VTProIntegrationTestSimpleSharp application.
    /// </summary>
    public class SignalHelper : ISignalHelper
    {
        private readonly IEnumSignalTypeRegistry enumSignalTypeRegistry;
        private readonly ILogger consoleLogger;

        /// <summary>
        /// Constructor for the signal helper class
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
        /// <exception cref="ArgumentNullException"><paramref name="panel"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"></exception>
        public void SetLogicJoin<T>(PanelDevice panel, Enum join, T value)
        {
            if(join == null)
                throw new ArgumentNullException(nameof(join), $"[SignalHelper] SetSerialJoin: Enum is null, cannot set join.");

            if (panel == null)
                throw new ArgumentNullException(nameof(panel) ,$"[SignalHelper] SetSerialJoin: Device is null, cannot set join.");

            SigType signalType = enumSignalTypeRegistry.Get(join.GetType());
            
            ushort joinNumber = Convert.ToUInt16(join);

            consoleLogger.Log($"[SignalHelper] Setting Join '{Convert.ToUInt16(join)}' on device '{panel.IPID}' as '{value}'");

            switch (signalType)
            {
                case SigType.Bool:

                    if (typeof(T) == typeof(bool))
                    {
                        panel.SetDigitalSignal(joinNumber, (bool)(object)value);
                    }
                    else
                        throw new ArgumentException($"[SignalHelper] Error: Type mismatch when attempting to set join for a given Enum: {join}, and Value: {value}");
                    break;

                case SigType.String:

                    if (typeof(T) == typeof(string))
                    {
                        panel.SetSerialSignal(joinNumber, (string)(object)value);
                    }
                    else
                        throw new ArgumentException($"[SignalHelper] Error: Type mismatch when attempting to set join for a given Enum: {join}, and Value: {value}");
                    break;

                case SigType.UShort:

                    if (typeof(T) == typeof(ushort))
                    {
                        panel.SetAnalogSignal(joinNumber, (ushort)(object)value);
                    }
                    else
                        throw new ArgumentException($"[SignalHelper] Error: Type mismatch when attempting to set join for a given Enum: {join}, and Value: {value}");
                    break;

                case SigType.NA:
                        throw new ArgumentException($"[SignalHelper] Error: Cannot process a NA type! Type is: {join.GetType()}");
            }
        }

        /// <summary>
        /// Sees if a button is pressed on a rising edge. (Not Smart Object)
        /// </summary>
        /// <param name="args">The signal received</param>
        /// <returns>Bool, If the signal was a rising edge</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public bool IsRisingEdge(SignalEventData args)
        {
            if (args.SignalType != SigType.Bool || args.DigitalValue == null)
                throw new ArgumentNullException("[SignalHelper] Cant check if non-digital signal is a Rising Edge");

            return (bool)args.DigitalValue;
        }

        /// <summary>
        /// Sees if a button is pressed on a rising edge. (Not Smart Object)
        /// </summary>
        /// <param name="args">The signal received</param>
        /// <returns>Bool, If the signal was a rising edge</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public bool IsRisingEdge(SigEventArgs args)
        {
            if (args.Sig.Type != eSigType.Bool)
                throw new ArgumentNullException("[SignalHelper] Cant check if non-digital signal is a Rising Edge");

            return args.Sig.BoolValue;
        }
    }
}
