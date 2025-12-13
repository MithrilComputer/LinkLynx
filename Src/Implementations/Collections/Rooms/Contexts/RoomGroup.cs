using LinkLynx.Core.Interfaces.Utility.Debugging.Logging;
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
        private readonly List<RoomObject> rooms = new List<RoomObject>();

        /// <summary>
        /// Logger instance for logging purposes. :D
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// The constructor for a <see cref="RoomGroup"/>.
        /// </summary>
        public RoomGroup(ILogger logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// The constructor for a <see cref="RoomGroup"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public RoomGroup(ILogger logger, IEnumerable<RoomObject> rooms)
        {
            this.logger = logger;

            if (rooms == null)
                throw new ArgumentNullException(nameof(rooms),$"[RoomGroup] Error: Attempted to initialize RoomGroup with null rooms list.");

            foreach (RoomObject room in rooms)
            {
                AddRoom(room);
            }
        }

        /// <summary>
        /// Adds a <see cref="RoomObject"/> to the RoomGroup.
        /// </summary>
        public void AddRoom(RoomObject room)
        {
            if (room == null)
            {
                logger.Log($"[RoomGroup] Cannot add a null RoomObject to the RoomGroup: {room.GetType().Name}");
                return;
            }

            if (rooms.Contains(room))
            {
                logger.Log($"[RoomGroup] Warning: Cant add Room '{room.RoomName}' with ID '{room.RoomID}' is already in the RoomGroup.");
                return;
            }

            rooms.Add(room);
        }

        /// <summary>
        /// Removes a <see cref="RoomObject"/> from the RoomGroup.
        /// </summary>
        public void RemoveRoom(RoomObject room)
        {
            if (room == null)
            {
                logger.Log($"[RoomGroup] Cannot remove a null RoomObject from the RoomGroup: {room.GetType().Name}");
                return;
            }

            if (!rooms.Contains(room))
            {
                logger.Log($"[RoomGroup] Warning: Cant remove Room '{room.RoomName}' with ID '{room.RoomID}' as it does not exist in the RoomGroup.");
                return;
            }

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

            logger.Log($"[RoomGroup] Warning: Room with name '{roomName}' not found in RoomGroup.");

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

            logger.Log($"[RoomGroup] Warning: Room with ID '{roomID}' not found in RoomGroup.");

            return null;
        }
    }
}
