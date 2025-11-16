using System;
using Newtonsoft.Json;

namespace Glitch9.IO.RESTApi
{
    /// <summary>
    /// Added 2025.04.15 to handle more setting properties for the RESTClient.
    /// This class is used to configure the RESTClient settings.
    /// </summary>
    public class RESTClientSettings
    {
        public JsonSerializerSettings JsonSettings { get; set; } = JsonUtils.DefaultSettings;
        public SSEParser SSEParser { get; set; } = new();
        public RESTLogger Logger { get; set; } = new("RESTClient", RESTApiV3.Config.kDefaultLogLevel);
        public TimeSpan Timeout { get; set; } = RESTApiV3.Config.kDefaultTimeout;
        public bool AllowBodyWithDELETE { get; set; } = false;
    }
}