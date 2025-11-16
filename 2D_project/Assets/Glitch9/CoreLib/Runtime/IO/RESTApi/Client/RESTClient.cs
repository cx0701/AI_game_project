using System;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine.Networking;
// ReSharper disable InconsistentNaming

namespace Glitch9.IO.RESTApi
{
    /// <summary>
    /// A REST client class for handling various types of REST API requests.
    /// </summary>
    public class RESTClient
    {
        // --- Client Properties --------------------------------------------------
        public JsonSerializerSettings JsonSettings => clientSettings.JsonSettings;
        public RESTLogger Logger => clientSettings.Logger;
        public RESTLogLevel LogLevel => Logger.LogLevel;
        public TimeSpan Timeout => clientSettings.Timeout;
        public SSEParser SSEParser => clientSettings.SSEParser;
        public bool AllowBodyWithDELETE => clientSettings.AllowBodyWithDELETE;

        // --- The Settings Object ------------------------------------------------
        protected readonly RESTClientSettings clientSettings;

        // --- Variables ----------------------------------------------------------
        public string LastRequest { get; set; } = "";
        public string LastEndpoint { get; set; } = "";

        /// <summary>
        /// Constructor to initialize RESTClient with optional JSON settings.
        /// </summary>
        /// <param name="jsonSettings">Custom JSON serializer settings.</param>
        /// <param name="sseParser">Custom SSE parser.</param>
        /// <param name="logger">Custom logger.</param>
        public RESTClient(RESTClientSettings clientSettings = null)
            => this.clientSettings = clientSettings ?? new RESTClientSettings();

        /// <summary>
        /// Sends a POST request with a body and no response body.
        /// </summary>
        /// <typeparam name="TReqBody">Request body type.</typeparam>
        /// <param name="request">Request object.</param>
        /// <returns>Response result.</returns>
        public virtual UniTask<RESTResponse> POST<TReqBody>(RESTRequest<TReqBody> request)
            => RESTApiV3.SendRequestAsync(request, UnityWebRequest.kHttpVerbPOST, this);

        /// <summary>
        /// Sends a POST request with a generic request and response type.
        /// </summary>
        /// <typeparam name="TReqBody">Request body type.</typeparam>
        /// <typeparam name="TResBody">Response body type.</typeparam>
        /// <param name="request">Request object.</param>
        /// <returns>Response result.</returns>
        public virtual UniTask<RESTResponse<TResBody>> POST<TReqBody, TResBody>(RESTRequest<TReqBody> request)
            => RESTApiV3.SendRequestAsync<TReqBody, TResBody>(request, UnityWebRequest.kHttpVerbPOST, this);

        public virtual UniTask<RESTResponse<TResBody>> POST<TResBody>(RESTRequest request)
            => RESTApiV3.SendRequestAsync<TResBody>(request, UnityWebRequest.kHttpVerbPOST, this);

        /// <summary>
        /// Sends a PUT request with a generic request type and default response and error types.
        /// </summary>
        /// <typeparam name="TReqBody">Request body type.</typeparam>
        /// <param name="request">Request object.</param>
        /// <returns>Response result.</returns>
        public virtual UniTask<RESTResponse> PUT<TReqBody>(RESTRequest<TReqBody> request)
            => RESTApiV3.SendRequestAsync(request, UnityWebRequest.kHttpVerbPUT, this);

        /// <summary>
        /// Sends a PUT request with a generic request and response type.
        /// </summary>
        /// <typeparam name="TReqBody">Request body type.</typeparam>
        /// <typeparam name="TResBody">Response body type.</typeparam>
        /// <param name="request">Request object.</param>
        /// <returns>Response result.</returns>
        public virtual UniTask<RESTResponse<TResBody>> PUT<TReqBody, TResBody>(RESTRequest<TReqBody> request)
            => RESTApiV3.SendRequestAsync<TReqBody, TResBody>(request, UnityWebRequest.kHttpVerbPUT, this);

        /// <summary>
        /// Sends a GET request with a generic request type and default response and error types.
        /// </summary>
        /// <typeparam name="TResBody">Response body type.</typeparam>
        /// <param name="request">Request object.</param>
        /// <returns>Response result.</returns>
        public virtual UniTask<RESTResponse<TResBody>> GET<TResBody>(RESTRequest request)
            => RESTApiV3.SendRequestAsync<TResBody>(request, UnityWebRequest.kHttpVerbGET, this);

        /// <summary>
        /// Sends a GET request with a generic request and response type.
        /// </summary>
        /// <typeparam name="TReqBody">Request body type.</typeparam>
        /// <typeparam name="TResBody">Response body type.</typeparam>
        /// <param name="request">Request object.</param>
        /// <returns>Response result.</returns>
        public virtual UniTask<RESTResponse<TResBody>> GET<TReqBody, TResBody>(RESTRequest<TReqBody> request)
            => RESTApiV3.SendRequestAsync<TReqBody, TResBody>(request, UnityWebRequest.kHttpVerbGET, this);

        /// <summary>
        /// Sends a DELETE request with a generic request and response type.
        /// </summary>
        /// <typeparam name="TReqBody">Request body type.</typeparam>
        /// <typeparam name="TResBody">Response body type.</typeparam>
        /// <param name="request">Request object.</param>
        /// <returns>Response result.</returns>
        public virtual UniTask<RESTResponse<TResBody>> DELETE<TReqBody, TResBody>(RESTRequest<TReqBody> request)
             => RESTApiV3.SendRequestAsync<TReqBody, TResBody>(request, UnityWebRequest.kHttpVerbDELETE, this);

        /// <summary>
        /// Sends a DELETE request with a generic request type and default response and error types.
        /// </summary>
        /// <typeparam name="TReqBody">Request body type.</typeparam>
        /// <param name="request">Request object.</param>
        /// <returns>Response result.</returns>
        public virtual UniTask<RESTResponse> DELETE<TReqBody>(RESTRequest<TReqBody> request)
            => RESTApiV3.SendRequestAsync(request, UnityWebRequest.kHttpVerbDELETE, this);

        public virtual UniTask<RESTResponse<TResBody>> DELETE<TResBody>(RESTRequest request)
            => RESTApiV3.SendRequestAsync<TResBody>(request, UnityWebRequest.kHttpVerbDELETE, this);

        public virtual UniTask<RESTResponse> DELETE(RESTRequest request)
            => RESTApiV3.SendRequestAsync(request, UnityWebRequest.kHttpVerbDELETE, this);


        /// <summary>
        /// Sends a HEAD request with a generic request type and default response and error types.
        /// </summary>
        /// <typeparam name="TReqBody">Request body type.</typeparam>
        /// <param name="request">Request object.</param>
        /// <returns>Response result.</returns>
        public virtual UniTask<RESTResponse> HEAD<TReqBody>(RESTRequest<TReqBody> request)
            => RESTApiV3.SendRequestAsync(request, UnityWebRequest.kHttpVerbHEAD, this);

        /// <summary>
        /// Sends a HEAD request with a generic request and response type.
        /// </summary>
        /// <typeparam name="TReqBody">Request body type.</typeparam>
        /// <typeparam name="TResBody">Response body type.</typeparam>
        /// <param name="request">Request object.</param>
        /// <returns>Response result.</returns>
        public virtual UniTask<RESTResponse<TResBody>> HEAD<TReqBody, TResBody>(RESTRequest<TReqBody> request)
            => RESTApiV3.SendRequestAsync<TReqBody, TResBody>(request, UnityWebRequest.kHttpVerbHEAD, this);

        /// <summary>
        /// Sends a PATCH request with a generic request type and default response and error types.
        /// </summary>
        /// <typeparam name="TReqBody">Request body type.</typeparam>
        /// <param name="request">Request object.</param>
        /// <returns>Response result.</returns>
        public virtual UniTask<RESTResponse> PATCH<TReqBody>(RESTRequest<TReqBody> request)
            => RESTApiV3.SendRequestAsync(request, RESTApiV3.Config.kPatchMethod, this);

        public virtual UniTask<RESTResponse<TResBody>> PATCH<TResBody>(RESTRequest request)
            => RESTApiV3.SendRequestAsync<TResBody>(request, RESTApiV3.Config.kPatchMethod, this);

        public virtual UniTask<RESTResponse> PATCH(RESTRequest request)
            => RESTApiV3.SendRequestAsync(request, RESTApiV3.Config.kPatchMethod, this);

        /// <summary>
        /// Sends a PATCH request with generic request, response, and error types.
        /// </summary>
        /// <typeparam name="TReqBody">Request body type.</typeparam>
        /// <typeparam name="TResBody">Response body type.</typeparam>
        /// <param name="request">Request object.</param>
        /// <returns>Response result.</returns>
        public virtual UniTask<RESTResponse<TResBody>> PATCH<TReqBody, TResBody>(RESTRequest<TReqBody> request)
            => RESTApiV3.SendRequestAsync<TReqBody, TResBody>(request, RESTApiV3.Config.kPatchMethod, this);
    }
}
