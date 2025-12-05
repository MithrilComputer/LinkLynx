using LinkLynx.Core.CrestronWrappers;

namespace LinkLynx.Core.Interfaces.Utility.Dispatching
{
    /// <summary>
    /// The IJoinInstanceRouter interface defines a contract for routing signal events to the appropriate join instances on a panel device.
    /// </summary>
    public interface IJoinInstanceRouter
    {
        /// <summary>
        /// Routes the given signal event data to the appropriate join instance on the specified panel device.
        /// </summary>
        void Route(TouchPanelDevice panel, SignalEventData data);
    }
}
