using Glitch9.IO.Files;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Networking;

namespace Glitch9.IO.RESTApi
{
    internal static class UnityWebRequestFactory
    {
        internal static UnityWebRequest Create<TReqBody>(RESTRequest<TReqBody> req, string method, RESTClient client)
        {
            CreateInternal(req, method, client);
            SetHeaders(req, client);
            return req.WebRequest;
        }

        internal static UnityWebRequest Create(RESTRequest req, string method, RESTClient client)
        {
            CreateInternal(req, method, client);
            SetHeaders(req, client);
            return req.WebRequest;
        }

        private static void SetHeaders(RESTRequest req, RESTClient client)
        {
            bool includeContentTypeHeader = req.MIMEType == MIMEType.Json;

            if (!req.IgnoreLogs && client.LogLevel.RequestHeader())
            {
                using (StringBuilderPool.Get(out StringBuilder sb))
                {
                    foreach (RESTHeader header in req.GetHeaders(includeContentTypeHeader))
                    {
                        sb.AppendLine(header.Name.Contains("Auth") ? $"{header.Name}: [ApiKey]" : $"{header.Name}: {header.Value}");
                        req.WebRequest.SetRequestHeader(header);
                    }
                    client.Logger.LogRequestHeaders(sb.ToString());
                }
            }
            else
            {
                foreach (RESTHeader header in req.GetHeaders(includeContentTypeHeader))
                {
                    req.WebRequest.SetRequestHeader(header);
                }
            }
        }

        private static void CreateInternal<TReqBody>(RESTRequest<TReqBody> req, string method, RESTClient client)
        {
            string url = req.Endpoint;
            bool isStream = req.StreamHandler != null;
            MIMEType contentType = req.MIMEType;
            TimeSpan timeout = req.Timeout ?? client.Timeout;

            if (contentType == MIMEType.Json)
            {
                req.WebRequest = new UnityWebRequest(url, method)
                {
                    downloadHandler = CreateDownloadHandler(req, client)
                };

                if (!isStream) req.Timeout = timeout;

                if (method != UnityWebRequest.kHttpVerbGET && (client.AllowBodyWithDELETE || method != UnityWebRequest.kHttpVerbDELETE))
                {
                    if (req.HasBody)
                    {
                        byte[] body = req.SerializeBody(client) ?? throw new Issue(ExceptionType.InvalidRequest, "Failed to encode body of this request.");
                        req.WebRequest.uploadHandler = new UploadHandlerRaw(body);
                    }
                }
            }
            else if (contentType == MIMEType.WWWForm)
            {
                req.WebRequest = UnityWebRequest.Post(url, req.Form ?? throw new Issue(ExceptionType.InvalidRequest, "Failed to encode form data of this request."));
                if (!isStream) req.Timeout = timeout;
            }
            else if (contentType == MIMEType.MultipartForm)
            {
                List<IMultipartFormSection> formData = req.SerializeBodyAsMultipart(client) ?? throw new Issue(ExceptionType.InvalidRequest, "Failed to encode form data of this request.");
                req.WebRequest = UnityWebRequest.Post(url, formData);
                if (!isStream) req.Timeout = timeout;
            }
            else
            {
                throw new Issue(ExceptionType.InvalidRequest, $"Unsupported content type: {contentType}");
            }
        }

        private static void CreateInternal(RESTRequest req, string method, RESTClient client)
        {
            string url = req.Endpoint;
            bool isStream = req.StreamHandler != null;
            MIMEType contentType = req.MIMEType;
            TimeSpan timeout = req.Timeout ?? client.Timeout;

            if (contentType == MIMEType.Json)
            {
                req.WebRequest = new UnityWebRequest(url, method)
                {
                    //timeout = (int)timeout.TotalSeconds,
                    downloadHandler = CreateDownloadHandler(req, client)
                };
                if (!isStream) req.Timeout = timeout;
            }
            else if (contentType == MIMEType.WWWForm)
            {
                req.WebRequest = UnityWebRequest.Post(url, req.Form ?? throw new Issue(ExceptionType.InvalidRequest, "Failed to encode form data of this request."));
                if (!isStream) req.Timeout = timeout;
            }
            else
            {
                throw new Issue(ExceptionType.InvalidRequest, $"Unsupported content type: {contentType}");
            }
        }

        private static DownloadHandler CreateDownloadHandler(RESTRequest req, RESTClient client)
        {
            return req.StreamHandler switch
            {
                ITextStreamHandler text => new TextStreamHandlerBuffer(client, text, req.IgnoreLogs),
                BinaryStreamHandler binary => new BinaryStreamHandlerBuffer(client, binary, req.IgnoreLogs),
                AudioStreamHandler audio => new AudioStreamHandlerBuffer(client, audio, req.IgnoreLogs),
                _ => new DownloadHandlerBuffer()
            };
        }
    }
}
