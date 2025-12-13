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

        ILogger logger;

        public GeneralDeviceGroup(List<DeviceContext> devices, , ILogger logger)
        {
            this.devices = devices;
            this.logger = logger;
        }

        /// <summary>
        /// Adds a device context to this room.
        /// </summary>
        public RoomObject AddDevice(DeviceContext device)
        {
            if (device == null)
            {
                logger.Log($"[RoomObject, Name: {RoomName}, ID: {RoomID}] Error: Can't Add a null Device");
                return this;
            }

            devices.Add(device);

            return this;
        }

    }
}
