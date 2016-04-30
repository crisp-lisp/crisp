using System;
using Packet.Enums;
using Packet.Interfaces.Server;

namespace Packet.Server
{
    public abstract class HttpRequest : IHttpRequest
    {
        public HttpMethod Method { get; }

        public IHttpVersion Version { get; }

        public string Url { get; set; }

        protected HttpRequest(HttpMethod method, IHttpVersion version)
        {
            Method = method;
            Version = version;
        }
    }
}
