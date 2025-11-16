using Glitch9.CoreLib.IO.Audio;
using Glitch9.IO.Files;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;


namespace Glitch9.IO.RESTApi
{
    public class RESTRequest<T> : RESTRequest
    {
        public T Body { get; set; }
        public override bool HasBody => true;
        public RESTRequest(string url, T body) :
            base(url, body is RESTRequestBody reqBody ? reqBody.options : null)
            => Body = body;
    }

    public class RESTRequest
    {
        /// <summary>
        /// Creates a RESTRequest with the given endpoint.
        /// This is used when you are sending a request with empty body.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static RESTRequest Temp(string url, RESTRequestOptions options = null) => new(url, options);

        public string Endpoint { get; set; }

        /// <summary>
        /// The headers of the request
        /// </summary>
        public List<RESTHeader> Headers { get; protected set; } = new();

        /// <summary>
        /// Holds a reference to a UnityWebRequest object, which manages the request to the remote server.
        /// </summary>
        public UnityWebRequest WebRequest { get; set; }

        /// <summary>
        /// The form data of the request
        /// </summary>
        public WWWForm Form { get; set; }

        /// <summary>
        /// If the request has a body or not
        /// </summary>
        public virtual bool HasBody => false;

        // --- options ------------------------------------------------
        protected readonly RESTRequestOptions _options;

        public string Id => _options.Id;

        /// <summary>
        /// The local sender of the request.
        /// </summary>
        public string Sender => _options.Sender;

        /// <summary>
        /// The directory path where the file will be downloaded.
        /// </summary>
        public string OutputPath => _options.OutputPath;

        public CancellationToken? Token { get => _options.Token; set => _options.Token = value; }

        /// <summary>
        /// The content type of the request. Default is Json
        /// </summary>
        public virtual MIMEType MIMEType => _options.MIMEType;

        /// <summary>
        /// The number of retries of the request. Default is 3
        /// </summary>
        public int MaxRetry => _options.MaxRetry;

        /// <summary>
        /// Seconds of delay to make a retry. Default is 1
        /// </summary>
        public float RetryDelayInSec => _options.RetryDelayInSec;

        /// <summary>
        /// The custom timeout for the request.
        /// </summary>{
        public TimeSpan? Timeout { get => _options.Timeout; set => _options.Timeout = value; }

        /// <summary>
        /// If this is set, 
        /// it means you are requesting a stream response from the server. 
        /// </summary>
        public IStreamHandler StreamHandler => _options.StreamHandler;

        /// <summary>
        /// The audio format of the expected audio response. Default is null.
        /// </summary>
        public AudioFormat OutputAudioFormat => _options.OutputAudioFormat;

        /// <summary>
        /// Returns true if the request is a stream request.
        /// </summary>
        public bool IsStreamRequest => StreamHandler != null;
        public bool IgnoreLogs => _options.IgnoreLogs;


        public RESTRequest() { _options = new(); }
        public RESTRequest(string url, RESTRequestOptions options = null)
        {
            Endpoint = url;
            _options = options ?? new();
        }

        public IEnumerable<RESTHeader> GetHeaders(bool includeContentTypeHeader)
        {
            if (includeContentTypeHeader) yield return MIMEType.GetHeader();
            foreach (RESTHeader header in Headers) yield return header;
        }

        public void AddHeader(RESTHeader header) => Headers.Add(header);
        public void Cancel()
        {
            if (Token == null)
            {
                Debug.LogError("Cancellation token is not set. Cannot cancel the request.");
                return;
            }

            if (Token.Value.IsCancellationRequested) return;
            Token.Value.ThrowIfCancellationRequested();
            Token = null;
            Debug.Log($"Request cancelled: {this}");
        }

        #region Equality Members
        public static bool operator ==(RESTRequest left, RESTRequest right)
        {
            if (left is null && right is null) return true;
            if (left is null || right is null) return false;
            return left.Id == right.Id;
        }

        public static bool operator !=(RESTRequest left, RESTRequest right)
        {
            return !(left == right);
        }

        protected bool Equals(RESTRequest other)
        {
            return Equals(Id, other.Id);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((RESTRequest)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
        #endregion 
    }
}