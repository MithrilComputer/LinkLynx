using LinkLynx.Core.Signals;
using System;

namespace LinkLynx.Core.Attributes
{
    /// <summary>
    /// This attribute class associates enum's with a signal type (Digital, Analog, Serial)
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum, AllowMultiple = true)]
    public class SigTypeAttribute : Attribute
    {
        /// <summary>
        /// The eSigType associated with the enum.
        /// </summary>
        public SigType JoinType { get; }

        /// <summary>
        /// This attribute class associates enum's with a signal type (Digital, Analog, Serial)
        /// </summary>
        public SigTypeAttribute(SigType joinType)
        {
            JoinType = joinType;
        }
    }
}
