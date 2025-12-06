using LinkLynx.Implementations.Collections.Rooms.Logic;

namespace LinkLynx.Implementations.Collections.Rooms.Contexts
{
    /// <summary>
    /// A collection of RoomScript instances assigned to a specific RoomObject.
    /// </summary>
    public class RoomScriptGroup
    {
        /// <summary>
        /// The scripts assigned to the room.
        /// </summary>
        public List<RoomScript> Scripts { get; private set; }

        /// <summary>
        /// The room that is assigned to this script group.
        /// </summary>
        public RoomObject AssignedRoom { get; private set; }

        /// <summary>
        /// The RoomScriptGroup Constructor.
        /// </summary>
        /// <param name="room">The room to be parented to.</param>
        /// <param name="scripts">List of scripts being assigned to the room.</param>
        public RoomScriptGroup(RoomObject room, List<RoomScript> scripts)
        {
            Scripts = scripts;
            AssignedRoom = room;
        }

        /// <summary>
        /// Initializes all <see cref="RoomScript"/>s in the pool.
        /// </summary>
        public void InitializeRoomScripts()
        {
            foreach (RoomScript script in Scripts)
            {
                script.Initialize();
            }
        }

        /// <summary>
        /// Gets the first typed <see cref="RoomScript"/> from the pool given a type; Returns null if none found.
        /// </summary>
        /// <typeparam name="T">The type you're attempting to get.</typeparam>
        public T GetScriptFromType<T>() where T : RoomScript
        {
            foreach (RoomScript script in Scripts)
            {
                if (script is T typedScript)
                    return typedScript;
            }

            return null;
        }

        /// <summary>
        /// Gets all typed <see cref="RoomScript"/>s from the pool given a type; Returns empty list if none found.
        /// </summary>
        /// <typeparam name="T">The type you're attempting to get.</typeparam>
        public List<T> GetScriptsFromType<T>() where T : RoomScript
        {
            List<T> values = new List<T>();

            foreach (RoomScript script in Scripts)
            {
                if (script is T typedScript)
                    values.Add(typedScript);
            }

            return values;
        }
    }
}
