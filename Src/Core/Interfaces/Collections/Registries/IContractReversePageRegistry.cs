namespace LinkLynx.Core.Interfaces.Collections.Registries
{
    /// <summary>
    /// The Contract Reverse Page Registry is responsible for mapping contract joins to their respective page IDs.
    /// </summary>
    public interface IContractReversePageRegistry
    {
        /// <summary>
        /// Tries to register a contract join to a page ID.
        /// </summary>
        bool TryRegister(string contractJoin, ushort pageId);

        /// <summary>
        /// Gets a page ID given an input contract join. Gives ushort.MaxValue if the key was not found.
        /// </summary>
        bool TryGetPageId(string contractJoin, out ushort pageId);
    }
}
