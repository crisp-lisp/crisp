using Packet.Enums;
using Packet.Interfaces.Server;

namespace Packet.Server
{
    public abstract class HttpRequest : IHttpRequest
    {
        public abstract RequestType RequestType { get; }

        public abstract HttpMethod Method { get; }

        public string Url { get; set; }
    }
}
