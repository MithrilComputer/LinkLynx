using LinkLynx.Core.Src.Implementations.Collections.Domains.Contexts;
using LinkLynx.Implementations.Collections.Zones;

namespace LinkLynx.Implementations.Service.Domains
{
    public class DomainManager
    {
        private readonly List<ZoneObject> zones = new List<ZoneObject>();

        public DomainScriptGroup ScriptGroup { get; }

        public DomainManager(DomainScriptGroup scriptGroup)
        {
            ScriptGroup = scriptGroup;
        }

        public void AddZone(ZoneObject zone)
        {
            zones.Add(zone);
        }

        public void RemoveZone(ZoneObject zone)
        {
            zones.Remove(zone);
        }
    }
}
