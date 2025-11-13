using Crestron.SimplSharpPro;
using LinkLynx.Core.Logic.Pages;
using LinkLynx.Core.CrestronPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkLynx.Core.Interfaces.Collections.Dispatchers
{
    /// <summary>
    /// The IDigitalJoinDispatcher interface defines a contract for managing a collection of actions
    /// </summary>
    public interface IDigitalJoinDispatcher
    {
        /// <summary>
        /// Gets the number of registered join actions.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Try's to add a new action for the specified join ID.
        /// </summary>
        bool TryAdd(uint joinId, Action<PageLogicBase, SignalEventData> action);

        /// <summary>
        /// Checks if an action for the specified join ID exists.
        /// </summary>
        bool Contains(uint joinId);

        /// <summary>
        /// Gets the action associated with the specified join ID.
        /// </summary>
        Action<PageLogicBase, SignalEventData> Get(uint joinId);
    }
}
