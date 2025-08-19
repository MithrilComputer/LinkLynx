using Crestron.SimplSharpPro;
using System;

namespace LinkLynx.Core.Utility.Helpers
{
    /// <summary>
    /// Helps with some specific cases involved with JoinMapping
    /// </summary>
    public static class EnumHelper
    {
        /// <summary>
        /// Gets a signal type from an enum for better clarity. Enum must contain any of the words 'digital', 'analog', or 'serial'.
        /// The function is not case sensitive, so 'DiGiTal' will work just as well as as 'Digital'.
        /// </summary>
        /// <param name="joinEnum">The enum that represents the logic join.</param>
        public static eSigType GetSignalTypeFromEnum(Enum joinEnum)
        {
            if (joinEnum == null)
                throw new ArgumentNullException($"[EnumHelper] Error: Cant get signal from a null Enum: {nameof(joinEnum)}");

            Type enumType = joinEnum.GetType();
            eSigType signalType = LinkLynxServices.enumSignalTypeRegistry.Get(enumType);

            return signalType;
        }
    }
}
