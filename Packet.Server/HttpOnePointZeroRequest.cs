using System.Collections.Generic;

using Packet.Enums;

namespace Packet.Server
{
    public class HttpOnePointZeroRequest : HttpRequest
    {
        public Dictionary<string, string> Headers { get; set; }

        public byte[] RequestBody { get; set; }

        public HttpOnePointZeroRequest()
        {
            RequestType = RequestType.OnePointZero;
            Version = HttpVersion.OnePointZero;
        }
    }
}
