
using Newtonsoft.Json;

namespace Glitch9
{
    public static class JsonUtils
    {
        private static JsonSerializerSettings _defaultSettings;
        public static JsonSerializerSettings DefaultSettings => _defaultSettings ??= new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.None,
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore,
        };
    }
}