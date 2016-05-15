using System.Collections.Generic;

using Packet.Enums;
using Packet.Interfaces.Server;

namespace Packet.Server
{
    public class FullHttpRequest : HttpRequest, IFullHttpRequest
    {
        public Dictionary<string, string> Headers { get; set; }
        
        public byte[] RequestBody { get; set; }

        /// <summary>
        /// Initializes a new instance of a request conforming to HTTP/1.0 or newer.
        /// </summary>
        /// <param name="method">The HTTP method (verb) contained in the request.</param>
        /// <param name="version">The HTTP version of the request.</param>
        public FullHttpRequest(HttpMethod method, IHttpVersion version) 
            : base(method, version)
        {
        }
    }
}
