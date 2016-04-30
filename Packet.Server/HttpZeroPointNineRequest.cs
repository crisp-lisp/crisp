using Packet.Enums;

namespace Packet.Server
{
    /// <summary>
    /// Represents a HTTP/0.9 simple-format request.
    /// </summary>
    public class HttpZeroPointNineRequest : HttpRequest
    {
        public HttpZeroPointNineRequest()
        {
            RequestType = RequestType.ZeroPointNine;
            Method = HttpMethod.Get; // HTTP/0.9 has this method only.
            Version = HttpVersion.ZeroPointNine; // Only HTTP/0.9 has simple requests.
        }
    }
}
