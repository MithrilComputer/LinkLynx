using LinkLynx.Core.Interfaces.Collections.Registries;
using LinkLynx.Core.Interfaces.Utility.Helpers;
using System;

namespace LinkLynx.Implementations.Utility.Helpers
{
    /// <summary>
    /// Helps with some specific cases involved with JoinMapping
    /// </summary>
    public class EnumHelper : IEnumHelper
    {
        private readonly IEnumSignalTypeRegistry enumSignalTypeRegistry;

        /// <summary>
        /// Helps with some specific cases involved with JoinMapping
        /// </summary>
        public EnumHelper(IEnumSignalTypeRegistry enumSignalTypeRegistry)
        {
            this.enumSignalTypeRegistry = enumSignalTypeRegistry;
        }

        /// <summary>
        /// Gets a signal type from an enum for better clarity. Enum must contain any of the words 'digital', 'analog', or 'serial'.
        /// The function is not case sensitive, so 'DiGiTal' will work just as well as as 'Digital'.
        /// </summary>
        /// <param name="joinEnum">The enum that represents the logic join.</param>
        public Core.Signals.SigType GetSignalTypeFromEnum(Enum joinEnum)
        {
            Type enumType = joinEnum.GetType();
            Core.Signals.SigType signalType = enumSignalTypeRegistry.Get(enumType);

            return signalType;
        }
    }
}
