namespace Glitch9.AIDevKit.Editor.Pro
{
    /// <summary>
    /// Utility class for resolving model metadata.
    /// </summary>
    internal static class ModelMetaUtil
    {
        internal static ModelCatalogueEntry ResolveMeta(ModelCatalogueEntry entry)
        {
            AIProvider api = entry.Api;

            if (api == AIProvider.OpenAI) return OpenAIModelMeta.Resolve(entry);
            if (api == AIProvider.Google) return GoogleModelMeta.Resolve(entry);
            if (api == AIProvider.ElevenLabs) return ElevenLabsModelMeta.Resolve(entry);
            if (api == AIProvider.Ollama) return OllamaModelMeta.Resolve(entry);
            if (api == AIProvider.OpenRouter) return OpenRouterModelMeta.Resolve(entry);

            return entry;
        }

        internal static double ResolveCost(AIProvider api, string costAsString)
        {
            if (api == AIProvider.OpenRouter) return OpenRouterModelMeta.ResolveCost(costAsString);
            return 0;
        }

        internal static (string family, string familyVersion) ResolveFamily(AIProvider provider, string id, string name)
        {
            return ModelFamilyResolver.Resolve(provider, id, name);
        }

        internal static string RemoveSlashPrefix(string id)
        {
            if (id.Contains('/'))
            {
                string[] parts = id.Split('/');
                id = parts[1];
            }

            return id;
        }

        internal static bool IsOModel(string id)
        {
            id = id.ToLowerInvariant().Replace(" ", "-");
            if (id.StartsWith("o") && id.Length > 1 && char.IsDigit(id[1])) return true;
            return false;
        }
    }
}