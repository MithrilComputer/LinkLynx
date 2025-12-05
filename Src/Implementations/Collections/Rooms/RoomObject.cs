using LinkLynx.Core.CrestronPOCOs;
using LinkLynx.Implementations.Collections.Rooms.Logic;

namespace LinkLynx.Implementations.Collections.Rooms
{
    public class RoomObject
    {
        List<RoomScript> scripts = new List<RoomScript>();
        
        List<RoomObject> childRooms = new List<RoomObject>();

        List<TouchPanelDevice> touchPanels = new List<TouchPanelDevice>();


    }
}
