using System;

using Packet.Interfaces.Server;

namespace Packet.Server
{
    public abstract class HttpRequestParser : IHttpRequestParser
    {
        public IHttpRequestParser Successor { get; set;  }
        
        protected static bool ValidateUrl(string url)
        {
            // Validate URL, must be relative.
            Uri validUrl;
            return Uri.TryCreate(url, UriKind.Relative, out validUrl);
        }

        protected abstract IHttpRequest AttemptParse(byte[] request);

        protected abstract IHttpVersion AttemptGetVersion(string requestLine);

        public IHttpRequest Parse(byte[] request)
        {
            return AttemptParse(request) ?? Successor?.Parse(request);
        }

        public IHttpVersion GetVersion(string requestLine)
        {
            return AttemptGetVersion(requestLine) ?? Successor?.GetVersion(requestLine);
        }
    }
}
