namespace LinkLynx.Core.Signals
{
    /// <summary>
    /// Represents the direction of a signal, either incoming or outgoing.
    /// </summary>
    public enum SignalDirection
    {
        /// <summary>
        /// If the signal is incoming. Panel to processor. 
        /// </summary>
        Incoming,

        /// <summary>
        /// If the signal is outgoing. Processor to panel.
        /// </summary>
        Outgoing
    }
}
