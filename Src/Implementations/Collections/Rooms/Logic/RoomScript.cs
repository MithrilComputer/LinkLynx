namespace LinkLynx.Implementations.Collections.Rooms.Logic
{
    /// <summary>
    /// A base class for creating custom room scripts that define specific behaviors and logic for a RoomObject.
    /// </summary>
    public abstract class RoomScript
    {
        /// <summary>
        /// The parent room that this script is associated with.
        /// </summary>
        public RoomObject ParentRoom { get; }

        /// <summary>
        /// Initialization function called after the system is built.
        /// </summary>
        public virtual void Initialize() { }

        /// <summary>
        /// The RoomScript Constructor.
        /// </summary>
        /// <param name="parentRoom">The <see cref="RoomObject"/> parent bound to the script.</param>
        protected RoomScript(RoomObject parentRoom)
        {
            if (parentRoom == null)
                throw new ArgumentNullException(nameof(parentRoom), $"[RoomScript] Error: Cant make new room script without assigning a valid Parent Room!!");

            ParentRoom = parentRoom;
        }
    }
}
