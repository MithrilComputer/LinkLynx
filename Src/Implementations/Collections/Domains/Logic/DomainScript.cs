using LinkLynx.Implementations.Collections.Rooms;
using LinkLynx.Implementations.Service.Domains;

namespace LinkLynx.Implementations.Collections.Domains.Logic
{
    /// <summary>
    /// The base class for all domain level scripts.
    /// </summary>
    public abstract class DomainScript
    {
        /// <summary>
        /// The <see cref="DomainManager"/> that this script is associated with.
        /// </summary>
        public DomainManager ParentDomain { get; }

        public DomainScript(DomainManager parentDomain)
        {
            ParentDomain = parentDomain;
        }

        /// <summary>
        /// Initialization function called after the system is built.
        /// </summary>
        public virtual void Initialize()
        {

        }
    }
}
