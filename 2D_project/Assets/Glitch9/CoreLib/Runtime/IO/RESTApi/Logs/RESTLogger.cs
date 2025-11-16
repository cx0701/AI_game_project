namespace Glitch9.IO.RESTApi
{
    public class RESTLogger : DefaultLogger
    {
        internal static bool enabled = true;

        private static class Tags
        {
            internal const string kRequestEndpoint = "Request-{0}";
            internal const string kRequestHeaders = "Request-Header";
            internal const string kRequestDetails = "Request";
            internal const string kRequestBody = "Request-Body";
            internal const string kResponseDetails = "Response";
            internal const string kResponseBody = "Response-Body";
            internal const string kResponseError = "Response-Error";
            internal const string kResponseStream = "Response-Stream";
        }

        public void LogRequestEndpoint(string method, string url)
        {
            if (!enabled) return;
            if (!_logLevel.RequestEndpoint()) return;
            url = RESTEndpointParser.HideKeyFromEndpoint(url);
            Info(string.Format(Tags.kRequestEndpoint, method), url);
        }

        public void LogRequestBody(string body)
        {
            if (!enabled) return;
            if (!_logLevel.RequestBody()) return;
            Info(Tags.kRequestBody, body);
        }

        public void LogRequestDetails(string details)
        {
            if (!enabled) return;
            if (!_logLevel.RequestDetails()) return;
            Info(Tags.kRequestDetails, details);
        }

        public void LogRequestHeaders(string header)
        {
            if (!enabled) return;
            if (!_logLevel.RequestHeader()) return;
            Info(Tags.kRequestHeaders, header);
        }

        public void LogResponseContentType(string info)
        {
            if (!enabled) return;
            if (!_logLevel.ResponseDetails()) return;
            Info(Tags.kResponseDetails, $"Content-Type: {info}");
        }

        public void LogResponseDetails(string info)
        {
            if (!enabled) return;
            if (!_logLevel.ResponseDetails()) return;
            Info(Tags.kResponseDetails, info);
        }

        public void LogResponseBody(string body)
        {
            if (!enabled) return;
            if (!_logLevel.ResponseBody()) return;
            Info(Tags.kResponseBody, body);
        }

        public void LogResponseStream(string data)
        {
            if (!enabled) return;
            if (!_logLevel.ResponseStream()) return;
            Info(Tags.kResponseStream, data);
        }

        public void LogResponseError(string error)
        {
            if (!enabled) return;
            Error(Tags.kResponseError, error);
        }

        public void Verbose(string eventId, string message)
        {
            if (!enabled) return;
            Info(eventId, message);
        }

        public void Delta(string eventId, string message)
        {
            if (!enabled) return;
            Info(eventId, message);
        }

        private readonly RESTLogLevel _logLevel;
        public RESTLogLevel LogLevel => _logLevel;

        public RESTLogger(string tag, RESTLogLevel logLevel) : base(tag)
        {
            _logLevel = logLevel;
        }

        public override void Info(string message)
        {
            if (!enabled) return;
            base.Info(Tag, message);
        }

        public override void Warning(string message)
        {
            if (!enabled) return;
            base.Warning(Tag, message);
        }

        public override void Error(string message)
        {
            if (!enabled) return;
            base.Error(Tag, message);
        }

        public override void Info(object sender, string message)
        {
            if (!enabled) return;
            base.Info(sender, message);
        }

        public override void Warning(object sender, string message)
        {
            if (!enabled) return;
            base.Warning(sender, message);
        }

        public override void Error(object sender, string message)
        {
            if (!enabled) return;
            base.Error(sender, message);
        }

    }
}