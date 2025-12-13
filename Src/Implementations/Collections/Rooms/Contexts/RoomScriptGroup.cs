using LinkLynx.Core.Interfaces.Utility.Debugging.Logging;
using LinkLynx.Implementations.Collections.Rooms.Logic;

namespace LinkLynx.Implementations.Collections.Rooms.Contexts
{
    /// <summary>
    /// A collection of RoomScript instances assigned to a specific RoomObject.
    /// </summary>
    public sealed class RoomScriptGroup
    {
        /// <summary>
        /// The scripts assigned to the room.
        /// </summary>
        private readonly List<RoomScript> scripts = new List<RoomScript>();

        /// <summary>
        /// Logger instance for logging purposes. :D
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// The RoomScriptGroup Constructor.
        /// </summary>
        public RoomScriptGroup(ILogger logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// The RoomScriptGroup Constructor.
        /// </summary>
        public RoomScriptGroup(ILogger logger, IEnumerable<RoomScript> initialScripts)
        {
            this.logger = logger;

            if (initialScripts == null)
                throw new ArgumentNullException(nameof(initialScripts));

            foreach (var script in initialScripts)
            {
                AddScript(script);
            }
        }

        /// <summary>
        /// Adds a <see cref="RoomScript"/> to the pool.
        /// </summary>
        public void AddScript(RoomScript script)
        {
            scripts.Add(script);
        }

        /// <summary>
        /// Removes a <see cref="RoomScript"/> from the pool.
        /// </summary>
        public void RemoveScript(RoomScript script)
        {
            scripts.Remove(script);
        }

        /// <summary>
        /// Initializes all <see cref="RoomScript"/>s in the pool.
        /// </summary>
        public void InitializeRoomScripts()
        {
            foreach (RoomScript script in scripts)
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
            foreach (RoomScript script in scripts)
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

            foreach (RoomScript script in scripts)
            {
                if (script is T typedScript)
                    values.Add(typedScript);
            }

            return values;
        }
    }
}
