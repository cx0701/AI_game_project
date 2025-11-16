using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Glitch9.IO.RESTApi
{
    internal static class RESTRequestSerializationExtensions
    {
        internal static byte[] SerializeBody<TReqBody>(this RESTRequest<TReqBody> req, RESTClient client)
        {
            TReqBody body = req.Body;
            if (body == null) return null;

            string json = JsonConvert.SerializeObject(body, client.JsonSettings);
            if (!req.IgnoreLogs) client.Logger.LogRequestBody(json);
            return Encoding.UTF8.GetBytes(json);
        }

        internal static List<IMultipartFormSection> SerializeBodyAsMultipart<TReqBody>(this RESTRequest<TReqBody> req, RESTClient client)
        {
            if (req == null || req.Body == null) return null;

            List<IMultipartFormSection> formData = new();

            // use reflection to get all public properties of the request
            List<PropertyInfo> properties = PropertyInfoCache.Get<TReqBody>();
            JsonSerializer jsonSerializer = JsonSerializer.Create(client.JsonSettings);

            foreach (PropertyInfo prop in properties)
            {
                try
                {
                    //UnityEngine.Debug.LogWarning("0");
                    // Skip properties with [JsonIgnore]
                    JsonIgnoreAttribute jsonIgnore = AttributeCache<JsonIgnoreAttribute>.Get(prop);
                    if (jsonIgnore != null) continue;
                    //UnityEngine.Debug.LogWarning("1");
                    // Handle properties with [JsonProperty] for renaming
                    JsonPropertyAttribute jsonProp = AttributeCache<JsonPropertyAttribute>.Get(prop);
                    string key = jsonProp != null ? jsonProp.PropertyName : prop.Name;
                    //UnityEngine.Debug.LogWarning("2");
                    object value = prop.GetValue(req.Body);
                    if (value == null) continue;

                    // if the value if 'FormFile' or 'byte[]' then add it as a file section
                    if (value is FormFile || value is byte[])
                    {
                        //UnityEngine.Debug.LogWarning("3");
                        formData.Add(ValueToFormSection(key, value));
                        continue;
                    }

                    if (value is SystemLanguage lang)
                    {
                        value = lang.ToISOCode();
                    }

                    //UnityEngine.Debug.LogWarning("4");

                    // Serialize the value using the provided JsonSettings
                    string serializedValue;
                    using (var writer = new StringWriter())
                    {
                        jsonSerializer.Serialize(writer, value);
                        serializedValue = writer.ToString();

                        // Remove quotes from serialized string
                        serializedValue = serializedValue.Trim('"');
                    }
                    formData.Add(ValueToFormSection(key, serializedValue));

                    //UnityEngine.Debug.LogWarning("5");
                }
                catch (Exception ex)
                {
                    client.Logger.Error($"[MultipartForm] Failed to read property '{prop.Name}' of type {prop.PropertyType}: {ex}");
                    continue; // skip faulty property
                }
            }

            if (!req.IgnoreLogs && client.LogLevel.RequestBody())
            {
                using (StringBuilderPool.Get(out StringBuilder sb))
                {
                    sb.AppendLine("Multipart Form Data:");
                    foreach (IMultipartFormSection section in formData)
                    {
                        if (section is MultipartFormDataSection)
                        {
                            sb.AppendLine($"{section.sectionName}: {section.sectionData}");
                        }
                        else
                        {
                            sb.AppendLine($"{section.sectionName}: <file>");
                        }
                    }
                    client.Logger.LogRequestBody(sb.ToString());
                }
            }

            return formData;
        }


        private static IMultipartFormSection ValueToFormSection(string key, object value)
        {
            return value switch
            {
                byte[] bytes => new MultipartFormFileSection(key, bytes, "file", "application/octet-stream"),
                FormFile file => new MultipartFormFileSection(key, file.Data, file.FileName, file.ContentType.ToApiValue()),
                _ => new MultipartFormDataSection(key, Convert.ToString(value, CultureInfo.InvariantCulture))
            };
        }
    }
}