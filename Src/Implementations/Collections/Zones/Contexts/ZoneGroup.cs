using LinkLynx.Core.Interfaces.Utility.Debugging.Logging;
using LinkLynx.Implementations.Collections.Zones;

namespace LinkLynx.Core.Src.Implementations.Collections.Zones.Contexts
{
    public sealed class ZoneGroup
    {
        private readonly List<ZoneObject> zones = new List<ZoneObject>();

        private readonly ILogger logger;

        public ZoneGroup(ILogger logger)
        {
            this.logger = logger;
        }

        public ZoneGroup(ILogger logger, IEnumerable<ZoneObject> zones)
        {
            this.logger = logger;

            if (zones == null)
                throw new ArgumentNullException(nameof(zones));

            foreach (ZoneObject zone in zones)
            {
                AddZone(zone);
            }
        }

        public void AddZone(ZoneObject zone)
        {
            if (zone == null)
            {
                logger.Log($"[ZoneGroup] Warning: Attempted to add a null ZoneObject to ZoneGroup. Type: {zone.GetType().Name}");
                return;
            }

            if (zones.Contains(zone))
            {
                logger.Log($"[ZoneGroup] Warning: Attempted to add a ZoneObject that already exists in ZoneGroup. Zone Name: {zone.ZoneName}, Zone ID: {zone.ZoneID}");
                return;
            }

            zones.Add(zone);
        }

        public void RemoveZone(ZoneObject zone)
        {
            if (zone == null)
            {
                logger.Log($"[ZoneGroup] Warning: Attempted to remove a null ZoneObject from ZoneGroup. Type: {zone.GetType().Name}");
                return;
            }

            if (!zones.Contains(zone))
            {
                logger.Log($"[ZoneGroup] Warning: Attempted to remove a ZoneObject that does not exist in ZoneGroup. Zone Name: {zone.ZoneName}, Zone ID: {zone.ZoneID}");

                return;
            }

            zones.Remove(zone);
        }
    }
}
