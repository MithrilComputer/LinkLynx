using LinkLynx.Core.CrestronWrappers;
using LinkLynx.Core.Interfaces.Utility.Debugging.Logging;
using LinkLynx.Implementations.Collections.Rooms.Contexts;
using LinkLynx.Implementations.Collections.Rooms.Logic;
using LinkLynx.Implementations.Collections.Zones;
using LinkLynx.Implementations.Service.Domains;

namespace LinkLynx.Implementations.Collections.Rooms
{
    /// <summary>
    /// The RoomObject class represents a room within a zone, containing properties and methods to manage its state, scripts, devices, and child rooms.
    /// </summary>
    public class RoomObject
    {
        /// <summary>
        /// The unique identifier for this room.
        /// </summary>
        public ushort RoomID { get; }

        /// <summary>
        /// The name of this room.
        /// </summary>
        public string RoomName { get; }

        /// <summary>
        /// The script group associated with this room.
        /// </summary>
        public RoomScriptGroup ScriptGroup { get; }

        /// <summary>
        /// A list of child rooms contained within this room.
        /// </summary>
        private readonly List<RoomObject> childRooms = new List<RoomObject>();

        /// <summary>
        /// The parent room of this room, null if this is a top-level room.
        /// </summary>
        public RoomObject ParentRoom { get; }

        /// <summary>
        /// The parent zone that contains this room.
        /// </summary>
        public ZoneObject ParentZone { get; }

        /// <summary>
        /// The top level Domain.
        /// </summary>
        public DomainManager DomainManager { get; }

        /// <summary>
        /// A list of touch panel devices associated with this room.
        /// </summary>
        private readonly List<TouchPanelDevice> touchPanels = new List<TouchPanelDevice>();

        /// <summary>
        /// A list of device contexts associated with this room.
        /// </summary>
        private readonly List<DeviceContext> devices = new List<DeviceContext>();

        /// <summary>
        /// The logger instance for logging messages.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// The constructor for the RoomObject class.
        /// </summary>
        public RoomObject(ushort roomId, string roomName, DomainManager domainManager, ZoneObject parentZone, RoomScriptGroup scriptGroup, ILogger logger, RoomObject parentRoom = null)
        {
            RoomID = roomId;
            RoomName = roomName;
            DomainManager = domainManager;
            ParentZone = parentZone;
            ParentRoom = parentRoom;
            ScriptGroup = scriptGroup;

            this.logger = logger;
        }

        /// <summary>
        /// Initializes the room by invoking the Initialize method on each script in the ScriptGroup.
        /// </summary>
        public virtual void Initialize()
        {
            ScriptGroup.InitializeRoomScripts();
        }

        /// <summary>
        /// Adds a child room to this room.
        /// </summary>
        public RoomObject AddChildRoom(RoomObject room)
        {
            if (room == null)
            {
                logger.Log($"[RoomObject, Name: {RoomName}, ID: {RoomID}] Error: Can't add null Child Room");
                return this;
            }

            childRooms.Add(room);

            return this;
        }

        /// <summary>
        /// Removes a child room from this room.
        /// </summary>
        public RoomObject RemoveChildRoom(RoomObject room)
        {
            if (room == null)
            {
                logger.Log($"[RoomObject, Name: {RoomName}, ID: {RoomID}] Error: Can't remove null Child Room");
                return this;
            }

            childRooms.Remove(room);

            return this;
        }

        /// <summary>
        /// Adds a touch panel device to this room.
        /// </summary>
        public RoomObject AddTouchPanel(TouchPanelDevice panel)
        {
            if (panel == null)
            {
                logger.Log($"[RoomObject, Name: {RoomName}, ID: {RoomID}] Error: Can't add null Touch Panel Device");
                return this;
            }

            touchPanels.Add(panel);

            return this;
        }

        /// <summary>
        /// Removes a touch panel device from this room.
        /// </summary>
        public RoomObject RemoveTouchPanel(TouchPanelDevice panel)
        {
            if (panel == null)
            {
                logger.Log($"[RoomObject, Name: {RoomName}, ID: {RoomID}] Error: Can't remove null Touch Panel Device");
                return this;
            }

            touchPanels.Remove(panel);

            return this;
        }

        /// <summary>
        /// Adds a device context to this room.
        /// </summary>
        public RoomObject AddDevice(DeviceContext device)
        {
            if (device == null)
            {
                logger.Log($"[RoomObject, Name: {RoomName}, ID: {RoomID}] Error: Can't Add a null Device");
                return this;
            }

            devices.Add(device);

            return this;
        }

        /// <summary>
        /// Removes a device context from this room.
        /// </summary>
        public RoomObject RemoveDevice(DeviceContext device)
        {
            if (device == null)
            {
                logger.Log($"[RoomObject, Name: {RoomName}, ID: {RoomID}] Error: Can't Remove a null Device");
                return this;
            }

            devices.Remove(device);

            return this;
        }
    }
}
