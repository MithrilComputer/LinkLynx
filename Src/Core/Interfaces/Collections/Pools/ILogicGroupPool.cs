using Crestron.SimplSharpPro.DeviceSupport;
using LinkLynx.Core.CrestronPOCOs;
using LinkLynx.Implementations.Collections.PanelContexts;

namespace LinkLynx.Core.Interfaces.Collections.Pools
{
    public interface ILogicGroupPool
    {
        void RegisterPanel(PanelDevice device);

        void UnregisterPanel(PanelDevice device);

        PanelLogicGroup GetPanelLogicGroup(PanelDevice device);

        void InitializePanelLogic(PanelDevice device);

        void SetPanelDefaults(PanelDevice device);
    }
}
