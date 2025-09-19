using Crestron.SimplSharpPro.DeviceSupport;
using LinkLynx.Core.Collections;

namespace LinkLynx.Core.Interfaces
{
    public interface ILogicGroupPool
    {
        void RegisterPanel(BasicTriList device);

        void UnregisterPanel(BasicTriList device);

        PanelLogicGroup GetPanelLogicGroup(BasicTriList device);

        void InitializePanelLogic(BasicTriList device);

        void SetPanelDefaults(BasicTriList device);

        void Clear();
    }
}
