using LinkLynx.Core.CrestronPOCOs;
using LinkLynx.Core.Logic.Pages;
using System.Collections.Generic;

namespace LinkLynx.Core.Interfaces.Utility.Factories
{
    public interface IPageFactory
    {
        Dictionary<ushort, PageLogicBase> BuildPagesForPanel(PanelDevice panel);
    }
}
