using LinkLynx.PublicAPI.Interfaces;

namespace LinkLynx.PublicAPI.Implementations
{
    /// <summary>
    /// Used to locate the current LinkLynx Instance.
    /// </summary>
    public static class LinkLynxLocator
    {
        private static ILinkLynx? instance;

        /// <summary>
        /// The current LinkLynx instance.
        /// </summary>
        public static ILinkLynx? Current => instance ?? throw new InvalidOperationException(
            "LinkLynxLocator.Current has not been initialized.");

        /// <summary>
        /// Sets the current LinkLynx instance.
        /// </summary>
        /// <param name="linkLynx">The instance to set as the current instance.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void SetCurrent(ILinkLynx linkLynx)
        {
            if(linkLynx == null)
                throw new ArgumentNullException(nameof(linkLynx), "[LinkLynxLocator] Error: Cant assign a null LinkLynx Instance to the Current Instance!");

            instance = linkLynx;
        }
    }
}
