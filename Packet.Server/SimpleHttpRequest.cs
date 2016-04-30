using Packet.Enums;

namespace Packet.Server
{
    /// <summary>
    /// Represents a HTTP/0.9 simple-format request.
    /// </summary>
    public class SimpleHttpRequest : HttpRequest
    {
        public override RequestType RequestType => RequestType.Simple;

        public override HttpMethod Method => HttpMethod.Get; // HTTP/0.9 has this method only.
    }
}
