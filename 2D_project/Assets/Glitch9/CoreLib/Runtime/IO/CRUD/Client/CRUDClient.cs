using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Glitch9.IO.RESTApi
{
    /// <summary>
    /// REST API client for performing CRUD operations.
    /// </summary>
    public abstract partial class CRUDClient<TSelf> : RESTClient
        where TSelf : CRUDClient<TSelf>
    {
        /// <summary>
        /// The name of the API.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The version of the API.
        /// </summary>
        public string Version { get; }

        /// <summary>
        /// The beta version of the API if available.
        /// </summary>
        public string BetaVersion { get; }

        /// <summary>
        /// Delegate for handling exceptions that occur during an API request.
        /// </summary>
        /// <param name="exception">The exception that occurred.</param>
        public delegate void ExceptionHandler(string endpoint, Exception exception);

        /// <summary>
        /// Event invoked when an error occurs during an API request.
        /// </summary>
        public ExceptionHandler OnException { get; set; }

        /// <summary>
        /// Special logger for logging CRUD operations.
        /// </summary>
        public CRUDLogger CRUDLogger => (CRUDLogger)Logger;

        public string BaseUrl { get; }

        // Fields 
        private readonly Func<string> apiKeyGetter;
        private readonly AutoParam autoApiKey;
        private readonly AutoParam autoVersionParam;
        private readonly AutoParam autoBetaParam;
        private readonly RESTHeader? betaHeader;
        private readonly RESTHeader[] additionalHeaders;
        private readonly string apiKeyQueryKey;
        private readonly string apiKeyHeaderKey;
        private readonly string apiKeyHeaderFormat;

        public string GetApiKey() => apiKeyGetter?.Invoke();

        // Constructors
        protected CRUDClient(CRUDClientSettings clientSettings) : base(clientSettings)
        {
            if (clientSettings == null) throw new ArgumentNullException(nameof(clientSettings));
            if (string.IsNullOrEmpty(clientSettings.Name)) throw new ArgumentException("API name must be set.", nameof(clientSettings));
            if (string.IsNullOrEmpty(clientSettings.BaseURL)) throw new ArgumentException("Base URL must be set.", nameof(clientSettings));

            if (clientSettings.AutoApiKey != AutoParam.Unset)
            {
                apiKeyGetter = clientSettings.ApiKeyGetter;
                if (apiKeyGetter == null) throw new ArgumentException("API key getter must be set.", nameof(clientSettings));
                string apiKey = clientSettings.ApiKeyGetter?.Invoke();
                if (string.IsNullOrEmpty(apiKey)) throw new ArgumentException("API key is not provided. Make sure GetApiKey() is implemented correctly.");

                if (clientSettings.AutoApiKey == AutoParam.Query)
                {
                    if (string.IsNullOrEmpty(clientSettings.ApiKeyQueryKey)) throw new ArgumentException("API key param must be set.", nameof(clientSettings));
                    apiKeyQueryKey = clientSettings.ApiKeyQueryKey;
                }
            }

            if (clientSettings.AutoBetaParam != AutoParam.Unset)
            {
                if (clientSettings.AutoBetaParam == AutoParam.Header)
                {
                    if (clientSettings.BetaHeader == null) throw new ArgumentException("Beta header must be set.", nameof(clientSettings));
                    betaHeader = clientSettings.BetaHeader.Value;
                }
            }

            Name = clientSettings.Name;
            Version = clientSettings.Version;
            BetaVersion = clientSettings.BetaApiVersion;

            BaseUrl = clientSettings.BaseURL;
            autoApiKey = clientSettings.AutoApiKey;
            autoVersionParam = clientSettings.AutoVersionParam;
            autoBetaParam = clientSettings.AutoBetaParam;
            additionalHeaders = clientSettings.AdditionalHeaders;

            apiKeyHeaderKey = clientSettings.CustomApiKeyHeaderKey ?? RESTHeader.kDefaultAuthHeaderName;
            apiKeyHeaderFormat = clientSettings.CustomApiKeyHeaderFormat ?? RESTHeader.kAuthHeaderFormat;
        }


        // Override Methods -------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Parses the error res from the API.
        /// Override this method to customize the error message parsing logic.
        /// </summary>
        /// <param name="errorJson"></param>
        /// <returns>The error message.</returns>
        protected abstract string ParseErrorMessage(string errorJson);

        /// <summary>
        /// Override this method to handle the status of a <see cref="CRUDMethod.Delete"/> operation.
        /// </summary>
        /// <typeparam name="TResBody"></typeparam>
        /// <param name="res"></param>
        /// <returns></returns>
        protected abstract bool IsDeletedPredicate(RESTResponse res);

        // CRUD Operations
        private async UniTask<TResBody> CreateAsync<TReqBody, TResBody>(RESTRequest<TReqBody> req, string endpoint)
        {
            ThrowIf.ArgumentIsNull(req, "Create Request");
            ThrowIf.EndpointIsNull(endpoint);
            req.Endpoint = endpoint;

            Type reqType = req.GetType();
            if (!req.IgnoreLogs && LogLevel.RequestDetails()) CRUDLogger.Create(reqType);

            RESTResponse<TResBody> res = await POST<TReqBody, TResBody>(req);
            ThrowIf.ResultIsNull(res);

            return FinalizeResponse(CRUDMethod.Create, res);
        }

        private async UniTask<RESTResponse> CreateAsync<TReqBody>(RESTRequest<TReqBody> req, string endpoint)
        {
            ThrowIf.ArgumentIsNull(req, "Create Request");
            ThrowIf.EndpointIsNull(endpoint);
            req.Endpoint = endpoint;

            Type reqType = req.GetType();
            if (!req.IgnoreLogs && LogLevel.RequestDetails()) CRUDLogger.Create(reqType);

            RESTResponse res = await POST(req);
            ThrowIf.ResultIsNull(res);

            return res;
        }

        // Modify an object using GenerativeAI API with dynamic endpoint resolution.
        private async UniTask<TResBody> UpdateAsync<TReqBody, TResBody>(RESTRequest<TReqBody> req, string endpoint)
        {
            ThrowIf.ArgumentIsNull(req, "Update Request");
            ThrowIf.EndpointIsNull(endpoint);
            req.Endpoint = endpoint;

            if (!req.IgnoreLogs && LogLevel.RequestDetails()) CRUDLogger.Update(req.GetType());
            RESTResponse<TResBody> res = await POST<TReqBody, TResBody>(req);
            ThrowIf.ResultIsNull(res);

            return FinalizeResponse(CRUDMethod.Update, res);
        }

        private async UniTask<TResBody> RetrieveAsync<TResBody>(RESTRequest req, string endpoint)
        {
            ThrowIf.ArgumentIsNull(req, "Get Request");
            ThrowIf.EndpointIsNull(endpoint);
            req.Endpoint = endpoint;

            if (!req.IgnoreLogs && LogLevel.RequestDetails()) CRUDLogger.Retrieve(typeof(TResBody));
            RESTResponse<TResBody> res = await GET<TResBody>(req);
            ThrowIf.ResultIsNull(res);

            return FinalizeResponse(CRUDMethod.Retrieve, res);
        }

        private async UniTask<TResBody> PatchAsync<TResBody>(RESTRequest req, string endpoint)
        {
            ThrowIf.ArgumentIsNull(req, "Patch Request");
            ThrowIf.EndpointIsNull(endpoint);
            req.Endpoint = endpoint;

            if (!req.IgnoreLogs && LogLevel.RequestDetails()) CRUDLogger.Patch(typeof(TResBody));
            RESTResponse<TResBody> res = await PATCH<TResBody>(req);
            ThrowIf.ResultIsNull(res);

            return FinalizeResponse(CRUDMethod.Patch, res);
        }

        // Cancel an object using GenerativeAI API with dynamic endpoint resolution.
        private async UniTask<TResBody> CancelAsync<TResBody>(RESTRequest req, string endpoint)
        {
            ThrowIf.ArgumentIsNull(req, "Cancel Request");
            ThrowIf.EndpointIsNull(endpoint);
            req.Endpoint = endpoint;

            if (!req.IgnoreLogs && LogLevel.RequestDetails()) CRUDLogger.Cancel(typeof(TResBody));
            RESTResponse<TResBody> res = await POST<TResBody>(req);
            ThrowIf.ResultIsNull(res);

            return FinalizeResponse(CRUDMethod.Cancel, res);
        }

        // Delete an object using GenerativeAI API with dynamic endpoint resolution.
        // If successful, the res body is empty.
        private async UniTask<bool> DeleteAsync<TResBody>(RESTRequest req, string endpoint)
        {
            ThrowIf.ArgumentIsNull(req, "Delete Request");
            ThrowIf.EndpointIsNull(endpoint);
            req.Endpoint = endpoint;

            if (!req.IgnoreLogs && LogLevel.RequestDetails()) CRUDLogger.Delete(typeof(TResBody));
            RESTResponse<TResBody> res = await DELETE<TResBody>(req);
            ThrowIf.ResultIsNull(res);

            return IsDeletedPredicate(res);
        }

        private async UniTask<bool> DeleteAsync<TReqBody>(RESTRequest<TReqBody> req, string endpoint)
        {
            ThrowIf.ArgumentIsNull(req, "Delete Request");
            ThrowIf.EndpointIsNull(endpoint);
            req.Endpoint = endpoint;

            if (!req.IgnoreLogs && LogLevel.RequestDetails()) CRUDLogger.Delete(typeof(TReqBody));
            RESTResponse res = await DELETE(req);
            ThrowIf.ResultIsNull(res);

            return res.HasBody;
        }

        // Retrieve a list of objects using a query with optional parameters.
        // List with body
        private async UniTask<QueryResponse<TResBody>> ListAsync<TReqBody, TResBody>(RESTRequest<TReqBody> req, string endpoint)
        {
            ThrowIf.ArgumentIsNull(req, "List Request");
            ThrowIf.EndpointIsNull(endpoint);
            req.Endpoint = endpoint;

            if (!req.IgnoreLogs && LogLevel.RequestDetails()) CRUDLogger.List(typeof(TResBody));
            RESTResponse<QueryResponse<TResBody>> res = await GET<TReqBody, QueryResponse<TResBody>>(req);
            ThrowIf.ResultIsNull(res);

            return FinalizeResponse(CRUDMethod.List, res);
        }

        // List with no body
        private async UniTask<QueryResponse<TResBody>> ListAsync<TResBody>(RESTRequest req, string endpoint)
        {
            ThrowIf.ArgumentIsNull(req, "List Request");
            ThrowIf.EndpointIsNull(endpoint);
            req.Endpoint = endpoint;

            if (!req.IgnoreLogs && LogLevel.RequestDetails()) CRUDLogger.List(typeof(TResBody));
            RESTResponse<QueryResponse<TResBody>> res = await GET<QueryResponse<TResBody>>(req);
            ThrowIf.ResultIsNull(res);

            return FinalizeResponse(CRUDMethod.List, res);
        }

        // --- Utility Methods --------------------------------------------------------------------------------------------------------------

        // Handle ress from API calls, converting them to the appropriate type.
        private TResBody FinalizeResponse<TResBody>(CRUDMethod method, RESTResponse<TResBody> res)
        {
            if (res.IsSuccess) return res.Body;
            HandleErrorLogging(method, res);
            return res.Body;
        }

        private QueryResponse<TResBody> FinalizeResponse<TResBody>(CRUDMethod method, RESTResponse<QueryResponse<TResBody>> res)
        {
            if (res.IsSuccess) return res.Body;
            HandleErrorLogging(method, res);
            return res.Body;
        }

        private void HandleErrorLogging(CRUDMethod method, RESTResponse res)
        {
            string failReason = res.ErrorMessage;
            if (string.IsNullOrEmpty(failReason)) failReason = "No fail reason provided.";
            string failMessage = $"<color=cyan>[{LastRequest}({method})]</color> Error: {failReason}";
            CRUDLogger.Error(failMessage);
        }

        // Handle exceptions from GenerativeAI API calls, potentially parsing error messages.
        public void HandleException(Exception exception)
        {
            //string exceptionMessage = exception.Message; 
            OnException?.Invoke(LastEndpoint, exception);
        }
    }
}