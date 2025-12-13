using LinkLynx.Implementations.Collections.Rooms;

namespace LinkLynx.Core.Src.Implementations.Collections.Rooms.Contexts
{
    /// <summary>
    /// The RoomGroup class manages a collection of <see cref="RoomObject"/> instances, providing methods to add, remove, and retrieve rooms by name or ID.
    /// </summary>
    public sealed class RoomGroup
    {
        /// <summary>
        /// The rooms contained in the RoomGroup.
        /// </summary>
        private readonly List<RoomObject> rooms;

        /// <summary>
        /// A read-only list of all <see cref="RoomObject"/>s in the RoomGroup.
        /// </summary>
        public IReadOnlyList<RoomObject> Rooms => rooms.AsReadOnly();

        /// <summary>
        /// The constructor for a <see cref="RoomGroup"/>.
        /// </summary>
        public RoomGroup(List<RoomObject> rooms)
        {
            this.rooms = rooms;
        }

        /// <summary>
        /// Adds a <see cref="RoomObject"/> to the RoomGroup.
        /// </summary>
        public void AddRoom(RoomObject room)
        {
            rooms.Add(room);
        }

        /// <summary>
        /// Removes a <see cref="RoomObject"/> from the RoomGroup.
        /// </summary>
        public void RemoveRoom(RoomObject room)
        {
            rooms.Remove(room);
        }

        /// <summary>
        /// Gets a <see cref="RoomObject"/> from the RoomGroup by its name.
        /// </summary>
        public RoomObject GetRoomFromName(string roomName)
        {
            foreach (RoomObject room in rooms)
            {
                if (room.RoomName == roomName)
                {
                    return room;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets a <see cref="RoomObject"/> from the RoomGroup by its ID.
        /// </summary>
        public RoomObject GetRoomFromID(ushort roomID)
        {
            foreach (RoomObject room in rooms)
            {
                if (room.RoomID == roomID)
                {
                    return room;
                }
            }
            return null;
        }
    }
}
