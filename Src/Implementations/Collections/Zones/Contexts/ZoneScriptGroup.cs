using LinkLynx.Implementations.Collections.Zones.Logic;

namespace LinkLynx.Implementations.Collections.Zones.Contexts
{
    /// <summary>
    /// The pool of <see cref="ZoneScript"/>s assigned to a specific <see cref="ZoneObject"/>.
    /// </summary>
    public sealed class ZoneScriptGroup
    {
        /// <summary>
        /// A list of all <see cref="ZoneScript"/>s in the pool.
        /// </summary>
        private readonly List<ZoneScript> scripts;

        /// <summary>
        /// The <see cref="ZoneObject"/> assigned to this pool.
        /// </summary>
        public ZoneObject AssignedZone { get; private set; }

        /// <summary>
        /// Constructor for a <see cref="ZoneScriptGroup"/>.
        /// </summary>
        public ZoneScriptGroup(ZoneObject zone, List<ZoneScript> scripts)
        {
            this.scripts = scripts;
            AssignedZone = zone;
        }

        /// <summary>
        /// Initializes all <see cref="ZoneScript"/>s in the pool.
        /// </summary>
        public void InitializeRoomScripts()
        {
            foreach (ZoneScript script in scripts)
            {
                script.Initialize();
            }
        }

        /// <summary>
        /// Adds a <see cref="ZoneScript"/> to the pool.
        /// </summary>
        public void AddScript(ZoneScript script)
        {
            scripts.Add(script);
        }


        /// <summary>
        /// Adds a <see cref="ZoneScript"/> to the pool.
        /// </summary>
        public void RemoveScript(ZoneScript script)
        {
            scripts.Remove(script);
        }

        /// <summary>
        /// Gets the first typed <see cref="ZoneScript"/> from the pool given a type; Returns null if none found.
        /// </summary>
        /// <typeparam name="T">The type you're attempting to get.</typeparam>
        public T GetScriptFromType<T>() where T : ZoneScript
        {
            foreach (ZoneScript script in scripts)
            {
                if (script is T typedScript)
                    return typedScript;
            }

            return null;
        }

        /// <summary>
        /// Gets all typed <see cref="ZoneScript"/>s from the pool given a type; Returns empty list if none found.
        /// </summary>
        /// <typeparam name="T">The type you're attempting to get.</typeparam>
        public List<T> GetScriptsFromType<T>() where T : ZoneScript
        {
            List<T> values = new List<T>();

            foreach (ZoneScript script in scripts)
            {
                if (script is T typedScript)
                    values.Add(typedScript);
            }

            return values;
        }
    }
}
