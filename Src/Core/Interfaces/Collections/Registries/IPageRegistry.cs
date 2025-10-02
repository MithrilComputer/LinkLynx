using Crestron.SimplSharpPro.DeviceSupport;
using LinkLynx.Core.Logic.Pages;
using System;
using System.Collections.Generic;

namespace LinkLynx.Core.Interfaces.Collections.Registries
{
    public interface IPageRegistry
    {
        void RegisterPage(ushort pageId, Func<BasicTriList, PageLogicBase> pageLogic);

        Func<BasicTriList, PageLogicBase> GetPage(ushort pageId);

        IReadOnlyDictionary<ushort, Func<BasicTriList, PageLogicBase>> GetAllRegistries();
    }
}
