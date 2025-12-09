using LinkLynx.Core.Interfaces.Utility.Debugging.Logging;
using LinkLynx.Implementations.Collections.Rooms;
using LinkLynx.Implementations.Collections.Zones.Contexts;
using LinkLynx.Implementations.Collections.Zones.Logic;
using LinkLynx.Implementations.Service.Domains;

namespace LinkLynx.Implementations.Collections.Zones
{
    /// <summary>
    /// The ZoneObject class represents a zone within the system, containing properties and methods to manage its state and associated logic and rooms.
    /// </summary>
    public class ZoneObject
    {
        /// <summary>
        /// The name of the Zone.
        /// </summary>
        public string ZoneName { get; set; }

        /// <summary>
        /// The unique ID of the Zone.
        /// </summary>
        public ushort ZoneID { get; set; }

        /// <summary>
        /// The top level Domain.
        /// </summary>
        public DomainManager DomainManager { get; }

        /// <summary>
        /// The <see cref="ZoneScriptGroup"/> assigned to this zone.
        /// </summary>
        public ZoneScriptGroup ScriptGroup { get; }

        /// <summary>
        /// A list of all instantiated rooms contained within this zone.
        /// </summary>
        private readonly List<RoomObject> rooms = new List<RoomObject>();

        /// <summary>
        /// The logger instance for logging messages.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// The constructor for the ZoneObject.
        /// </summary>
        public ZoneObject(ushort zoneId, string name, DomainManager domainManager, ILogger logger)
        {
            ZoneID = zoneId;
            ZoneName = name;
            DomainManager = domainManager;
            this.logger = logger;
        }

        /// <summary>
        /// Adds a <see cref="RoomObject"/> to this Zone.
        /// </summary>
        public ZoneObject AddRoom(RoomObject room)
        {
            if (room == null)
            {
                logger.Log($"[ZoneObject, Name: {ZoneName}, ID: {ZoneID}] Warning: Can't add null Room, Type: {room.GetType().Name}");
                return this;
            }

            rooms.Add(room);

            return this;
        }

        /// <summary>
        /// Removes a <see cref="RoomObject"/> from this Zone.
        /// </summary>
        public ZoneObject RemoveRoom(RoomObject room)
        {
            if (room == null)
            {
                logger.Log($"[ZoneObject, Name: {ZoneName}, ID: {ZoneID}] Warning: Can't remove null Room, Type: {room.GetType().Name}");
                return this;
            }

            rooms.Remove(room);

            return this;
        }
    }
}
