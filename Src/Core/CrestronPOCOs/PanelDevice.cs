using Crestron.SimplSharpPro.DeviceSupport;

namespace LinkLynx.Core.CrestronPOCOs
{
    // I did'nt want to make this class.
    // But Crestron aggressively protects all their types and blocks inheritance,
    // dependency inversion, and mocking because sharing APIs is apparently illegal.
    // So now I must create this wrapper because their SDK was designed in the stone age and never updated again.
    // Crestron hates joy, unit, mock tests and modern software engineering.
    // >:( 

    /// <summary>
    /// Lightweight wrapper for a Crestron <see cref="BasicTriList"/> panel device used for testing
    /// and abstraction. This class exposes basic identifying information without requiring
    /// a dependency on Crestron hardware at construction time.
    /// </summary>
    /// <remarks>
    /// This is primarily a data container and does not provide signal routing or connection
    /// management. It exists to break direct dependencies on <see cref="BasicTriList"/> in areas
    /// where mocking or dependency inversion is required.
    /// </remarks>
    public class PanelDevice
    {
        /// <summary>
        /// The original Crestron <see cref="BasicTriList"/> instance associated with this panel,
        /// if available.
        /// </summary>
        /// <remarks>
        /// This property may be <c>null</c> when the <see cref="PanelDevice"/> is created using
        /// the simplified constructor for testing or offline usage.
        /// </remarks>
        public BasicTriList BasicTriList { get; set; }

        /// <summary>
        /// Unique Crestron device ID of the touchpanel.
        /// </summary>
        public uint IPID { get; set; }

        /// <summary>
        /// Indicates whether the panel is currently online.
        /// </summary>
        /// <remarks>
        /// This state is not dynamically updated and reflects only the status at the time
        /// of instantiation unless updated manually.
        /// </remarks>
        public bool IsOnline { get; set; }

        /// <summary>
        /// Friendly name assigned to the panel.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Creates a new <see cref="PanelDevice"/> wrapper from an existing Crestron
        /// <see cref="BasicTriList"/> instance.
        /// </summary>
        /// <param name="panel">The Crestron panel to wrap.</param>
        public PanelDevice(BasicTriList panel)
        {
            IPID = panel.ID;
            IsOnline = panel.IsOnline;
            Name = panel.Name;
        }

        /// <summary>
        /// Creates a <see cref="PanelDevice"/> instance using manual panel metadata.
        /// </summary>
        /// <param name="deviceID">The Crestron device ID.</param>
        /// <param name="isOnline">Initial online status of the panel.</param>
        /// <param name="name">Friendly name associated with the panel.</param>
        /// <remarks>
        /// Use this constructor when running unit tests or working offline where no
        /// physical <see cref="BasicTriList"/> instance is available.
        /// </remarks>
        public PanelDevice(uint deviceID, bool isOnline, string name)
        {
            IPID = deviceID;
            IsOnline = isOnline;
            Name = name;
        }
    }
}
