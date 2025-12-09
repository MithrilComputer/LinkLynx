using LinkLynx.Implementations.Collections.Domains.Logic;

namespace LinkLynx.Core.Src.Implementations.Collections.Domains.Contexts
{
    public sealed class DomainScriptGroup
    {
        private readonly List<DomainScript> scripts = new List<DomainScript>();

        public DomainScriptGroup(List<DomainScript> scripts)
        {
            this.scripts = scripts;
        }

        public void InitializeDomainScripts()
        {
            foreach (DomainScript script in scripts)
            {
                script.Initialize();
            }
        }

        public void AddScript(DomainScript script)
        {
            scripts.Add(script);
        }

        public void RemoveScript(DomainScript script)
        {
            scripts.Remove(script);
        }

        public T GetScriptFromType<T>() where T : DomainScript
        {
            foreach (DomainScript script in scripts)
            {
                if (script is T typedScript)
                {
                    return typedScript;
                }
            }
            return null;
        }

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
