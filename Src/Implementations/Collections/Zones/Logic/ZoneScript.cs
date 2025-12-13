namespace LinkLynx.Implementations.Collections.Zones.Logic
{
    /// <summary>
    /// A base class for creating custom zone scripts that define specific behaviors and logic for a ZoneObject.
    /// </summary>
    public abstract class ZoneScript
    {
        /// <summary>
        /// The parent zone that this script is associated with.
        /// </summary>
        public ZoneObject ParentZone { get; }

        /// <summary>
        /// Initialization function called after the system is built.
        /// </summary>
        public virtual void Initialize() { }

        /// <summary>
        /// The constructor for a ZoneScript.
        /// </summary>
        protected ZoneScript(ZoneObject parentZone)
        {
            if (parentZone == null)
                throw new ArgumentNullException(nameof(parentZone), $"[ZoneScript] Error: Cant make new zone script without assigning a valid Parent Zone!!");

            ParentZone = parentZone;
        }
    }
}
