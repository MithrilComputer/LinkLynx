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
            string typeName = joinEnum.GetType().Name.ToLower();

            if (typeName.Contains("digital")) return eSigType.Bool;
            if (typeName.Contains("analog")) return eSigType.UShort;
            if (typeName.Contains("serial")) return eSigType.String;

            throw new ArgumentException($"Unknown signal type from enum name: {typeName}");
        }
    }
}
