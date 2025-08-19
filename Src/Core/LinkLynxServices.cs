using LinkLynx.Core.Collections.Pools;
using LinkLynx.Core.Utility.Dispatchers.Signals;
using LinkLynx.Core.Utility.Registries;

namespace LinkLynx.Core
{
    /// <summary>
    /// A facade for all utility singletons.
    /// </summary>
    public static class LinkLynxServices
    {
        /// <summary>
        /// The EnumSignalTypeRegistry singleton instance reference.
        /// </summary>
        internal static readonly EnumSignalTypeRegistry enumSignalTypeRegistry = EnumSignalTypeRegistry.Instance;

        /// <summary>
        /// The AnalogDispatcher singleton instance reference.
        /// </summary>
        internal static readonly AnalogDispatcher analogDispatcher = AnalogDispatcher.Instance;

        /// <summary>
        /// The DigitalDispatcher singleton instance reference.
        /// </summary>
        internal static readonly DigitalDispatcher digitalDispatcher = DigitalDispatcher.Instance;

        /// <summary>
        /// The SerialDispatcher singleton instance reference.
        /// </summary>
        internal static readonly SerialDispatcher serialDispatcher = SerialDispatcher.Instance;

        /// <summary>
        /// The ReversePageRegistry singleton instance reference.
        /// </summary>
        internal static readonly ReversePageRegistry reversePageRegistry = ReversePageRegistry.Instance;

        /// <summary>
        /// The PageRegistry singleton instance reference.
        /// </summary>
        internal static readonly PageRegistry pageRegistry = PageRegistry.Instance;

        /// <summary>
        /// The LogicGroupPool singleton instance reference.
        /// </summary>
        internal static readonly LogicGroupPool logicGroupPool = LogicGroupPool.Instance;

        /// <summary>
        /// The PanelPool singleton instance reference.
        /// </summary>
        internal static readonly PanelPool panelPool = PanelPool.Instance;
    }
}
