using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkLynx.Core.Signals
{
    /// <summary>
    /// signal Type Enum (Avoid's Crestron change issues)
    /// </summary>
    public enum eSigType
    {
        /// <summary>
        /// Not Applicable / Not Assigned
        /// </summary>
        NA = 0,

        /// <summary>
        /// Digital / Boolean Signal
        /// </summary>
        Bool = 1,

        /// <summary>
        /// Analog / UShort Signal
        /// </summary>
        UShort = 2,

        /// <summary>
        /// Serial / String Signal
        /// </summary>
        String = 3
    }
}
