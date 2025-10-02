using Crestron.SimplSharpPro.DeviceSupport;
using LinkLynx.Core.Logic.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkLynx.Core.Src.Core.Interfaces.Utility.Factories
{
    public interface IPageFactory
    {
        Dictionary<ushort, PageLogicBase> BuildPagesForPanel(BasicTriList panel);
    }
}
