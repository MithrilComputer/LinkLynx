namespace LinkLynx.Implementations.Collections.Rooms.Logic
{
    public abstract class RoomScript
    {
        public RoomObject ParentRoom { get; }

        public virtual void Initialize() { }
    }
}
