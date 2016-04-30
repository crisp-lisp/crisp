using Packet.Enums;

namespace Packet.Server
{
    /// <summary>
    /// Represents a HTTP/0.9 simple-format request.
    /// </summary>
    public class SimpleHttpRequest : HttpRequest
    {
        public SimpleHttpRequest()
            : base(HttpMethod.Get, new HttpVersion(0, 9))
        {
        }
    }
}
