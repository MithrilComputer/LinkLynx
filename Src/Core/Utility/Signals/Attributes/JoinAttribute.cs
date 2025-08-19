using System;

namespace LinkLynx.Core.Src.Core.Utility.Signals.Attributes
{
    /// <summary>
    /// This is the attribute class for giving methods a join key.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class JoinAttribute : Attribute
    {
        /// <summary>
        /// The join enum
        /// </summary>
        public object Join { get; }

        /// <summary>
        /// This is the attribute class for giving methods a join key.
        /// </summary>
        /// <param name="join">The join enum</param>
        public JoinAttribute(object join)
        {
            Join = join;
        }
    }
}
