using Crestron.SimplSharpPro.DeviceSupport;
using LinkLynx.Implementations.Collections.PanelContexts;

namespace LinkLynx.Core.Interfaces.Collections.Pools
{
    public interface ILogicGroupPool
    {
        void RegisterPanel(BasicTriList device);

        void UnregisterPanel(BasicTriList device);

        PanelLogicGroup GetPanelLogicGroup(BasicTriList device);

        void InitializePanelLogic(BasicTriList device);

        void SetPanelDefaults(BasicTriList device);
    }
}
