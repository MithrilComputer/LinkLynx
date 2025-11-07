using LinkLynx.Core.Signals;
using System;

namespace LinkLynx.Core.Interfaces.Utility.Helpers
{
    public interface IEnumHelper
    {
        SigType GetSignalTypeFromEnum(Enum joinEnum);
    }
}
