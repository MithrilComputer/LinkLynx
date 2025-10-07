using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.DeviceSupport;
using LinkLynx.Core.Signals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkLynx.Core.Src.Core.CrestronPOCOs
{
    // I did'nt want to make this class.
    // But Crestron protects all their classes, because they dont like sharing their API's
    // So now I must make a work around because everyone there is really old and really grumpy.
    // Crestron hates joy and unit / mach tests.
    // >:(

    /// <summary>
    /// Stores the information of a signal event from a panel. Its a wrapper class for the Crestron SigEventArgs used for mostly testing.
    /// </summary>
    public class SignalEventData
    {
        /// <summary>
        /// T
        /// </summary>
        public bool? DigitalValue { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public ushort? AnalogValue { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string SerialValue { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public SigType SignalType { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public uint SignalJoinID { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public SignalEventData(SigEventArgs sigEventArgs)
        {
            Sig sig = sigEventArgs.Sig;

            DigitalValue = sig.BoolValue;

            AnalogValue = sig.UShortValue;

            SerialValue = sig.StringValue;

            SignalType = (SigType)sig.Type;

            SignalJoinID = sig.Number;
        }

        /// <summary>
        /// 
        /// </summary>
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
