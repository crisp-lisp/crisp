using Packet.Enums;

namespace Packet.Server
{
    /// <summary>
    /// Represents a HTTP/0.9 simple-format request.
    /// </summary>
    public class SimpleHttpRequest : HttpRequest
    {
        public SimpleHttpRequest()
        {
            Method = HttpMethod.Get; // HTTP/0.9 has this method only.
            Version = HttpVersion.Http09; // Only HTTP/0.9 has simple requests.
        }
    }
}
