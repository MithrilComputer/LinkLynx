using LinkLynx.Core.Interfaces.Utility.Debugging.Logging;
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
        private readonly List<ZoneScript> scripts = new List<ZoneScript>();

        /// <summary>
        /// A logger instance for logging within the zone script group.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Constructor for a <see cref="ZoneScriptGroup"/>.
        /// </summary>
        public ZoneScriptGroup(ILogger logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Constructor for a <see cref="ZoneScriptGroup"/>.
        /// </summary>
        public ZoneScriptGroup(ILogger logger, IEnumerable<ZoneScript> initialScripts)
        {
            this.logger = logger;

            if (initialScripts == null)
                throw new ArgumentNullException(nameof(initialScripts), $"[ZoneScriptGroup] Error: Attempted to initialize ZoneScriptGroup with null initialScripts list.");

            foreach (ZoneScript script in initialScripts)
            {
                AddScript(script);
            }
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
            if (script == null)
            {
                logger.Log("[] Warning: Attempted to add a null ZoneScript to the ZoneScriptGroup.");
                return;
            }

            if (scripts.Contains(script))
            {
                logger.Log("Attempted to add a ZoneScript that is already in the ZoneScriptGroup.");
                return;
            }

            scripts.Add(script);
        }


        /// <summary>
        /// Adds a <see cref="ZoneScript"/> to the pool.
        /// </summary>
        public void RemoveScript(ZoneScript script)
        {
            if(script == null)
            {
                logger.Log($"[ZoneScriptGroup] Warning: Attempted to remove a null ZoneScript from the ZoneScriptGroup: {script.GetType().Name}");
                return;
            }

            if (!scripts.Contains(script))
            {
                logger.Log($"[ZoneScriptGroup] Warning: Attempted to remove a ZoneScript that does not exist in the ZoneScriptGroup: {script.GetType().Name}");
                return;
            }

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
