using Crestron.SimplSharpPro;
using System;

namespace LinkLynx.Core.Utility.Registries
{
    /// <summary>
    /// Helps with some specific cases involved with JoinMapping
    /// </summary>
    internal static class JoinHelper
    {
        /// <summary>
        /// Gets a signal type from an enum for better clarity.
        /// </summary>
        /// <param name="joinEnum">The enum that represents the logic join.</param>
        internal static eSigType GetSignalTypeFromEnum(Enum joinEnum)
        {
            string typeName = joinEnum.GetType().Name.ToLower();

            if (typeName.Contains("digital")) return eSigType.Bool;
            if (typeName.Contains("analog")) return eSigType.UShort;
            if (typeName.Contains("serial")) return eSigType.String;

            throw new ArgumentException($"Unknown signal type from enum name: {typeName}");
        }
    }
}
