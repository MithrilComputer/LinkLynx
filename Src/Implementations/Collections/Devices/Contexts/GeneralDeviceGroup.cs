using Crestron.SimplSharpPro.AudioDistribution;
using LinkLynx.Core.CrestronWrappers;
using LinkLynx.Core.Interfaces.Utility.Debugging.Logging;
using LinkLynx.Implementations.Collections.Rooms;

namespace LinkLynx.Core.Src.Implementations.Collections.Devices.Contexts
{
    public class GeneralDeviceGroup
    {
        /// <summary>
        /// A list of device contexts associated with this room.
        /// </summary>
        private readonly List<DeviceContext> devices = new List<DeviceContext>();

        /// <summary>
        /// A logger instance for logging within the device group.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// The GeneralDeviceGroup Constructor.
        /// </summary>
        public GeneralDeviceGroup(ILogger logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// The GeneralDeviceGroup Constructor.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public GeneralDeviceGroup(ILogger logger, IEnumerable<DeviceContext> devices)
        {
            this.logger = logger;

            if (devices == null)
                throw new ArgumentNullException(nameof(devices), $"[GeneralDeviceGroup] Error: Attempted to initialize GeneralDeviceGroup with null devices list.");

            foreach (DeviceContext device in devices)
            {
                AddDevice(device);
            }
        }

        /// <summary>
        /// Adds a device context to this room.
        /// </summary>
        public GeneralDeviceGroup AddDevice(DeviceContext device)
        {
            if (device == null)
            {
                logger.Log($"[GeneralDeviceGroup] Warning: Can't Add a null Device {device.GetType().Name}");
                return this;
            }

            if (devices.Contains(device))
            {
                logger.Log($"[GeneralDeviceGroup] Warning: Attempted to add duplicate device {device.Device.GetType().Name} to GeneralDeviceGroup.");
                return this;
            }

            devices.Add(device);

            return this;
        }

        /// <summary>
        /// Removes a device context from this room.
        /// </summary>
        public GeneralDeviceGroup RemoveDevice(DeviceContext device)
        {
            if (device == null)
            {
                logger.Log($"[GeneralDeviceGroup] Warning: Can't Remove a null Device {device.GetType().Name}");
                return this;
            }

            if (!devices.Contains(device))
            {
                logger.Log($"[GeneralDeviceGroup] Warning: Attempted to remove a device {device.Device.GetType().Name} that does not exist in GeneralDeviceGroup.");
                return this;
            }

            devices.Remove(device);

            return this;
        }
    }
}
