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
        public ushort RoomID { get; private set; }

        /// <summary>
        /// The name of this room.
        /// </summary>
        public string RoomName { get; private set; }

        /// <summary>
        /// The script group associated with this room.
        /// </summary>
        public RoomScriptGroup ScriptGroup { get; private set; }

        /// <summary>
        /// A list of child rooms contained within this room.
        /// </summary>
        private readonly List<RoomObject> childRooms = new List<RoomObject>();

        /// <summary>
        /// The parent room of this room, null if this is a top-level room.
        /// </summary>
        public RoomObject ParentRoom { get; private set; }

        /// <summary>
        /// The parent zone that contains this room.
        /// </summary>
        public ZoneObject ParentZone { get; private set; }

        /// <summary>
        /// The top level Domain.
        /// </summary>
        public DomainManager DomainManager { get; private set; }

        /// <summary>
        /// A list of touch panel devices associated with this room.
        /// </summary>
        private readonly List<TouchPanelDevice> touchPanels = new List<TouchPanelDevice>();

        /// <summary>
        /// A list of device contexts associated with this room.
        /// </summary>
        private readonly List<DeviceContext> devices = new List<DeviceContext>();

        private ILogger logger;

        public virtual void Initialize()
        {
            foreach(RoomScript script in ScriptGroup.Scripts)
            {
                try
                {
                    script.Initialize();
                }
                catch(Exception ex)
                {
                    logger.Log($"[RoomObject, Name: {RoomName}, ID: {RoomID}] Warning: Can't Initialize Script: {script.GetType().FullName}");
                }
            }
        }

        public RoomObject(ushort roomId, string roomName, DomainManager domainManager, ZoneObject parentZone, ILogger logger, RoomObject parentRoom = null)
        {
            RoomID = roomId;
            RoomName = roomName;
            DomainManager = domainManager;
            ParentZone = parentZone;
            ParentRoom = parentRoom;

            this.logger = logger;
        }

        public RoomObject SetRoomScriptGroup(RoomScriptGroup scriptGroup)
        {
            if (scriptGroup == null)
                throw new ArgumentNullException(nameof(scriptGroup));

            ScriptGroup = scriptGroup;

            return this;
        }

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
