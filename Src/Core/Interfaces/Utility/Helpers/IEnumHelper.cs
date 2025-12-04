using LinkLynx.Core.Signals;

namespace LinkLynx.Core.Interfaces.Utility.Helpers
{
    /// <summary>
    /// Provides helper methods for working with enums representing signal types.
    /// </summary>
    public interface IEnumHelper
    {
        /// <summary>
        /// Gets the corresponding <see cref="SigType"/> for a given enum value.
        /// A <see cref="SigType"/> indicating the signal type associated with the enum.
        /// </summary>
        SigType GetSignalTypeFromEnum(Enum joinEnum);
    }
}
