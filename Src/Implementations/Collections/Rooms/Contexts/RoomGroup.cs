using LinkLynx.Implementations.Collections.Rooms;

namespace LinkLynx.Core.Src.Implementations.Collections.Rooms.Contexts
{
    public sealed class RoomGroup
    {

        private readonly List<RoomObject> rooms;

        public RoomGroup(List<RoomObject> rooms)
        {
            this.rooms = rooms;
        }
    }
}
