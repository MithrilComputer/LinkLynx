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

        /// <summary>
        /// The constructor for the <see cref="DomainScript"/>.
        /// </summary>
        protected DomainScript(DomainManager parentDomain)
        {
            if (parentDomain == null)
                throw new ArgumentNullException(nameof(parentDomain), $"[DomainScript] Error: Cant make new domain script without assigning a valid Parent Domain!!");

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
