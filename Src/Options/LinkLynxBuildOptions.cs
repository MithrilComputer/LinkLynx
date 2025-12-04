namespace LinkLynx.Core.Options
{
    /// <summary>
    /// Defines some building options for the LinkLynx framework.
    /// </summary>
    public class LinkLynxBuildOptions
    {
        /// <summary>
        /// This option sets whether any given panel is automaticaly registered to the control system upon being registered to the LinkLynx Framework.
        /// </summary>
        public bool AutoRegisterPanelsToControlSystem { get; set; } = true;

        /// <summary>
        /// Constructs a new set of build
        /// </summary>
        /// <param name="autoRegisterPanelsToControlSystem"></param>
        public LinkLynxBuildOptions(bool autoRegisterPanelsToControlSystem = true)
        {
            AutoRegisterPanelsToControlSystem = autoRegisterPanelsToControlSystem;
        }
    }
}
