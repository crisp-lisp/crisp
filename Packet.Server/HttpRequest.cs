using Packet.Enums;
using Packet.Interfaces.Server;

namespace Packet.Server
{
    public abstract class HttpRequest : IHttpRequest
    {
        public HttpMethod Method { get; }

        public IHttpVersion Version { get; }

        public string Url { get; set; }

        /// <summary>
        /// Initializes a new instance of a HTTP request.
        /// </summary>
        /// <param name="method">The HTTP method (verb) contained in the request.</param>
        /// <param name="version">The HTTP version of the request.</param>
        protected HttpRequest(HttpMethod method, IHttpVersion version)
        {
            Method = method;
            Version = version;
        }
    }
}
