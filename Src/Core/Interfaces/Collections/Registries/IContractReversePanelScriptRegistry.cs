namespace LinkLynx.Core.Interfaces.Collections.Registries
{
    public interface IContractReversePanelScriptRegistry
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
