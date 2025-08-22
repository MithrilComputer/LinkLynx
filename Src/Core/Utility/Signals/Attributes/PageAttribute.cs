using System;

namespace LinkLynx.Signals.Attributes
{
    /// <summary>
    /// This is the attribute class for giving page logic classes an internal ID.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class PageAttribute : Attribute
    {
        /// <summary>
        /// The ID associated with the page.
        /// </summary>
        public ushort Id { get; }

        /// <summary>
        /// This is the attribute class for giving page logic classes an internal ID.
        /// </summary>
        public PageAttribute(ushort id) 
        {
            Id = id;
        } 
    }
}
