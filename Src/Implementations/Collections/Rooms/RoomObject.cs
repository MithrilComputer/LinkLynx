using Crestron.SimplSharpPro.AudioDistribution;
using LinkLynx.Core.CrestronWrappers;
using LinkLynx.Core.Interfaces.Utility.Debugging.Logging;
using LinkLynx.Implementations.Collections.Rooms.Contexts;
using LinkLynx.Implementations.Collections.Rooms.Logic;
using LinkLynx.Implementations.Collections.Zones;
using LinkLynx.Implementations.Service.Domains;

namespace LinkLynx.Implementations.Collections.Rooms
{
    public class RoomObject
    {
        public ushort RoomID { get; private set; }

        public string RoomName { get; private set; }

        public RoomScriptGroup ScriptGroup { get; private set; }

        private List<RoomObject> childRooms = new List<RoomObject>();

        public RoomObject ParentRoom { get; private set; }

        public ZoneObject ParentZone { get; private set; }

        public DomainManager DomainManager { get; private set; }

        private List<TouchPanelDevice> touchPanels = new List<TouchPanelDevice>();

        private List<DeviceContext> devices = new List<DeviceContext>();

        private ILogger logger;

        public virtual void Initialize()
        {
            foreach(RoomScript script in ScriptGroup.Scripts)
            {
                try
                {
                    //script.Initalize(); TODO
                }
                catch(Exception ex)
                {
                    logger.Log($"[RoomObject, Name: {RoomName}, ID: {RoomID}] Warning: Can't Initialize Script: {script.GetType().FullName}");
                }
            }
        }

        public virtual void Start()
        {
            foreach (RoomScript script in ScriptGroup.Scripts)
            {
                try
                {
                    //TODO script.Start();
                }
                catch (Exception ex)
                {
                    logger.Log($"[RoomObject, Name: {RoomName}, ID: {RoomID}] Warning: Can't Start Script: {script.GetType().FullName} ");
                }
            }
        }

        public virtual void Stop()
        {
            foreach (RoomScript script in ScriptGroup.Scripts)
            {
                try
                {
                    //TODO script.Stop();
                }
                catch (Exception ex)
                {
                    logger.Log($"[RoomObject, Name: {RoomName}, ID: {RoomID}] Warning: Can't Stop Script: {script.GetType().FullName} ");
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
