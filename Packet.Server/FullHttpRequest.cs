using System.Collections.Generic;

using Packet.Enums;

namespace Packet.Server
{
    /// <summary>
    /// Represents a HTTP/1.0 request.
    /// </summary>
    public class FullHttpRequest : HttpRequest
    {
        /// <summary>
        /// Gets a name-value
        /// </summary>
        public Dictionary<string, string> Headers { get; set; }

        public byte[] RequestBody { get; set; }

        public FullHttpRequest()
        {
            Version = HttpVersion.Http10;
        }
    }
}
