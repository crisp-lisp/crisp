using System;

using Packet.Interfaces.Server;

namespace Packet.Server
{
    public abstract class HttpRequestParser : IHttpRequestParser
    {
        public IHttpRequestParser Successor { get; set;  }
        
        /// <summary>
        /// Returns true if the given URL string is a valid relative URL, otherwise return false.
        /// </summary>
        /// <param name="url">The URL string to validate.</param>
        /// <returns></returns>
        protected static bool ValidateUrl(string url)
        {
            // Validate URL, must be relative.
            Uri validUrl;
            return Uri.TryCreate(url, UriKind.Relative, out validUrl);
        }

        /// <summary>
        /// Attempts to parse the given request data, returns null if parsing fails.
        /// </summary>
        /// <param name="request">The request data to attempt to parse.</param>
        /// <returns></returns>
        protected abstract IHttpRequest AttemptParse(byte[] request);

        /// <summary>
        /// Attempts to get the version from the given request line, returns null if parsing fails.
        /// </summary>
        /// <param name="requestLine">The request line to attempt to parse.</param>
        /// <returns></returns>
        protected abstract IHttpVersion AttemptGetVersion(string requestLine);

        public IHttpRequest Parse(byte[] request)
        {
            // Chain of responsibility.
            return AttemptParse(request) ?? Successor?.Parse(request);
        }

        public IHttpVersion GetVersion(string requestLine)
        {
            // Chain of responsibility.
            return AttemptGetVersion(requestLine) ?? Successor?.GetVersion(requestLine);
        }
    }
}
