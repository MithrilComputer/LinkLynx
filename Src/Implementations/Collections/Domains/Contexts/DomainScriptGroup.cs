using LinkLynx.Core.Interfaces.Utility.Debugging.Logging;
using LinkLynx.Implementations.Collections.Domains.Logic;

namespace LinkLynx.Core.Src.Implementations.Collections.Domains.Contexts
{
    /// <summary>
    /// A collection of DomainScript instances assigned to a specific Domain.
    /// </summary>
    public sealed class DomainScriptGroup
    {
        /// <summary>
        /// A list of the scripts assigned to the domain.
        /// </summary>
        private readonly List<DomainScript> scripts = new List<DomainScript>();

        /// <summary>
        /// A logger instance for logging purposes.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// A constructor for a DomainScriptGroup.
        /// </summary>
        public DomainScriptGroup(ILogger logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// A constructor for a DomainScriptGroup.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public DomainScriptGroup(ILogger logger, IEnumerable<DomainScript> scripts)
        {
            this.logger = logger;

            if (scripts == null)
                throw new ArgumentNullException(nameof(scripts), $"[DomainScriptGroup] Error: Attempted to initialize DomainScriptGroup with null scripts list.");

            foreach (DomainScript script in scripts)
            {
                AddScript(script);
            }
        }

        /// <summary>
        /// Initializes all scripts in the DomainScriptGroup.
        /// </summary>
        public void InitializeDomainScripts()
        {
            foreach (DomainScript script in scripts)
            {
                script.Initialize();
            }
        }

        /// <summary>
        /// Adds a script to the DomainScriptGroup.
        /// </summary>
        public void AddScript(DomainScript script)
        {
            if (script == null)
            {
                logger.Log($"[DomainScriptGroup] Warning: Attempted to add null script to DomainScriptGroup: {script.GetType().Name}");
                return;
            }

            if (scripts.Contains(script))
            {
                logger.Log($"[DomainScriptGroup] Warning: Attempted to add duplicate script of type {script.GetType().Name} to DomainScriptGroup.");
                return;
            }

            scripts.Add(script);
        }

        /// <summary>
        /// Removes a script from the DomainScriptGroup.
        /// </summary>
        public void RemoveScript(DomainScript script)
        {
            if (script == null)
            {
                logger.Log($"[DomainScriptGroup] Warning: Attempted to remove null script from DomainScriptGroup:  {script.GetType().Name}");
                return;
            }

            if (!scripts.Contains(script))
            {
                logger.Log($"[DomainScriptGroup] Warning: Attempted to remove non-existent script of type {script.GetType().Name} from DomainScriptGroup.");
                return;
            }

            scripts.Remove(script);
        }

        /// <summary>
        /// Gets the first script of the specified type T from the DomainScriptGroup, null if none are found.
        /// </summary>
        public T GetScriptFromType<T>() where T : DomainScript
        {
            foreach (DomainScript script in scripts)
            {
                if (script is T typedScript)
                {
                    return typedScript;
                }
            }

            logger.Log($"[DomainScriptGroup] Warning: No script of type {typeof(T).Name} found in DomainScriptGroup.");

            return null;
        }

        /// <summary>
        /// Gets all scripts of the specified type T from the DomainScriptGroup, empty list if none are found.
        /// </summary>
        public List<T> GetScriptsFromType<T>() where T : DomainScript
        {
            List<T> foundScripts = new List<T>();

            foreach (DomainScript script in scripts)
            {
                if (script is T typedScript)
                {
                    foundScripts.Add(typedScript);
                }
            }

            return foundScripts;
        }
    }
}
