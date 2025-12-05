using Crestron.SimplSharpPro;

namespace LinkLynx.Core.CrestronWrappers
{
    /// <summary>
    /// A wrapper class that holds context information about a device.
    /// </summary>
    public class DeviceContext
    {
        /// <summary>
        /// The device associated with this context.
        /// </summary>
        public GenericDevice Device { get; private set; }

        /// <summary>
        /// The DeviceContext constructor.
        /// </summary>
        /// <param name="device">The device to assign to the context.</param>
        public DeviceContext(GenericDevice device)
        {
            Device = device;
        }
    }
}
