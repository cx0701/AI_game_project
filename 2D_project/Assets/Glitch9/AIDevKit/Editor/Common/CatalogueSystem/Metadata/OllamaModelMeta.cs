namespace Glitch9.AIDevKit.Editor.Pro
{
    internal static class OllamaModelMeta
    {
        internal static ModelCatalogueEntry Resolve(ModelCatalogueEntry entry)
        {
            // Missing Properties:
            // ✓ Name
            // ✓ Capability
            // ✘ Version
            // ✘ CreatedAt
            // ✘ Description 
            // ✘ InputModality, OutputModality
            // ✘ InputTokenLimit, OutputTokenLimit 
            // ✓ Provider

            entry.Name = ModelNameResolver.ResolveFromId(entry.Id);
            //entry.Version = ModelMetaUtil.ResolveVersion(entry.Id);
            entry.Capability = ModelCapability.TextGeneration;
            entry.Provider = ModelProviderResolver.Resolve(entry.Id);

            return entry;
        }
    }
}