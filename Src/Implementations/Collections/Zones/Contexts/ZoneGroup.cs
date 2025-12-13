using LinkLynx.Core.Interfaces.Utility.Debugging.Logging;
using LinkLynx.Implementations.Collections.Zones;

namespace LinkLynx.Core.Src.Implementations.Collections.Zones.Contexts
{
    /// <summary>
    /// A collection of ZoneObject instances.
    /// </summary>
    public sealed class ZoneGroup
    {
        /// <summary>
        /// The zones contained in the ZoneGroup.
        /// </summary>
        private readonly List<ZoneObject> zones = new List<ZoneObject>();

        /// <summary>
        /// A logger instance for logging within the zone group.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// The ZoneGroup Constructor.
        /// </summary>
        /// <param name="logger"></param>
        public ZoneGroup(ILogger logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// The ZoneGroup Constructor.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public ZoneGroup(ILogger logger, IEnumerable<ZoneObject> zones)
        {
            this.logger = logger;

            if (zones == null)
                throw new ArgumentNullException(nameof(zones), $"[ZoneGroup] Error: can't contstruct a ZoneGroup with a null zones argument!");

            foreach (ZoneObject zone in zones)
            {
                AddZone(zone);
            }
        }

        /// <summary>
        /// Adds a ZoneObject to the ZoneGroup.
        /// </summary>
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

        /// <summary>
        /// Removes a ZoneObject from the ZoneGroup.
        /// </summary>
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
