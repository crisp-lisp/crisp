using System.Collections.Generic;

using Packet.Enums;
using Packet.Interfaces.Server;

namespace Packet.Server
{
    /// <summary>
    /// Represents a request conforming to HTTP/1.0 or newer.
    /// </summary>
    public class FullHttpRequest : HttpRequest
    {
        /// <summary>
        /// Gets or sets a dictionary of headers in the request.
        /// </summary>
        public Dictionary<string, string> Headers { get; set; }
        
        /// <summary>
        /// Gets or sets the request body.
        /// </summary>
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
