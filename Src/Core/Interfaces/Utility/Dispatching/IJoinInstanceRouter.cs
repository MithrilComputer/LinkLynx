using LinkLynx.Core.CrestronPOCOs;

namespace LinkLynx.Core.Src.Core.Interfaces.Utility.Dispatching
{
    public interface IJoinInstanceRouter
    {
        void Route(PanelDevice panel, SignalEventData data);
    }
}
