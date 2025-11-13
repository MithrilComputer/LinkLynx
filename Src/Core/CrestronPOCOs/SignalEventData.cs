using Crestron.SimplSharpPro;
using LinkLynx.Core.Signals;

namespace LinkLynx.Core.CrestronPOCOs
{
    // I did'nt want to make this class.
    // But Crestron aggressively protects all their types and blocks inheritance,
    // dependency inversion, and mocking because sharing APIs is apparently illegal.
    // So now I must create this wrapper because their SDK was designed in the stone age and never updated again.
    // Crestron hates joy, unit, mock tests and modern software engineering.
    // >:( 

    /// <summary>
    /// Stores the information of a signal event from a panel. Its a wrapper class for the Crestron SigEventArgs used for mostly testing.
    /// </summary>
    public class SignalEventData
    {
        /// <summary>
        /// Original Crestron <see cref="SigEventArgs"/> instance associated with this signal event
        /// </summary>
        /// <remarks>
        /// This is retained for compatibility with native Crestron APIs, but should be avoided in
        /// most cases in favor of the strongly typed properties(<see cref = "DigitalValue" />,
        /// <see cref="AnalogValue"/>, <see cref="SerialValue"/> and <see cref="SignalType"/>).
        /// </remarks>
        public SigEventArgs SigEventArgs { get; private set; }

        /// <summary>
        /// The Digital representation of the signal.
        /// </summary>
        public bool? DigitalValue { get; private set; }

        /// <summary>
        /// The Analog representation of the signal.
        /// </summary>
        public ushort? AnalogValue { get; private set; }

        /// <summary>
        /// The Serial representation of the signal.
        /// </summary>
        public string SerialValue { get; private set; }

        /// <summary>
        /// The type of signal that changed.
        /// </summary>
        public SigType SignalType { get; private set; }

        /// <summary>
        /// The join number that identifies which signal changed on the panel.
        /// </summary>
        public uint SignalJoinID { get; private set; }

        /// <summary>
        /// Creates a new <see cref="SignalEventData"/> instance from a native Crestron
        /// <see cref="SigEventArgs"/> event.
        /// </summary>
        /// <param name="sigEventArgs">The original Crestron event arguments.</param>
        public SignalEventData(SigEventArgs sigEventArgs)
        {
            Sig sig = sigEventArgs.Sig;

            DigitalValue = sig.BoolValue;

            AnalogValue = sig.UShortValue;

            SerialValue = sig.StringValue;

            SignalType = (SigType)sig.Type;

            SignalJoinID = sig.Number;

            SigEventArgs = sigEventArgs;
        }

        /// <summary>
        /// Creates a new <see cref="SignalEventData"/> instance using manually provided signal data.
        /// </summary>
        /// <param name="signalJoinID">The join number of the signal.</param>
        /// <param name="sigType">The Crestron signal type (Digital, Analog, or Serial).</param>
        /// <param name="digitalValue">Optional digital value if the signal is of type Digital.</param>
        /// <param name="analogValue">Optional analog value if the signal is of type Analog.</param>
        /// <param name="serialValue">Optional serial value if the signal is of type Serial.</param>
        public SignalEventData(uint signalJoinID, SigType sigType, bool? digitalValue = null, ushort? analogValue = null, string serialValue = null)
        {
            DigitalValue = digitalValue;

            AnalogValue = analogValue;

            SerialValue = serialValue;

            SignalType = sigType;

            SignalJoinID = signalJoinID;
        }
    }
}
