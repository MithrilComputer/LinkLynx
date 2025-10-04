using LinkLynx.Core.Logic.Pages;

namespace LinkLynx.Core.Interfaces.Utility.Dispatching
{
    public interface IAutoJoinRegistrar
    {
        void RegisterJoins<T>(ushort pageId) where T : PageLogicBase;
    }
}
