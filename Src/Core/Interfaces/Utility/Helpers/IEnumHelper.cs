using LinkLynx.Core.Signals;
using System;

namespace LinkLynx.Core.Interfaces.Utility.Helpers
{
    internal interface IEnumHelper
    {
        SigType GetSignalTypeFromEnum(Enum joinEnum);
    }
}
