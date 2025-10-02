using LinkLynx.Core.Signals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkLynx.Core.Src.Core.Interfaces.Utility.Helpers
{
    internal interface IEnumHelper
    {
        eSigType GetSignalTypeFromEnum(Enum joinEnum);
    }
}
