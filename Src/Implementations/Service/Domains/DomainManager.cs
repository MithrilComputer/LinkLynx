using LinkLynx.Core.Src.Implementations.Collections.Domains.Contexts;
using LinkLynx.Core.Src.Implementations.Collections.Zones.Contexts;
using LinkLynx.Implementations.Collections.Zones;

namespace LinkLynx.Implementations.Service.Domains
{
    public class DomainManager
    {
        public DomainScriptGroup ScriptGroup { get; }

        public ZoneGroup ZoneGroup { get; }

        public DomainManager(DomainScriptGroup scriptGroup, ZoneGroup zoneGroup)
        {
            ScriptGroup = scriptGroup;

            ZoneGroup = zoneGroup;
        }
    }
}
