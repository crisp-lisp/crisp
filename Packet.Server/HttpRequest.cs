using System;
using Packet.Enums;
using Packet.Interfaces.Server;

namespace Packet.Server
{
    public abstract class HttpRequest : IHttpRequest
    {
        public RequestType RequestType { get; set; }

        public HttpMethod Method { get; set; }

        public HttpVersion Version { get; set; }

        public string Url { get; set; }
    }
}
