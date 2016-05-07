using System.Collections.Generic;

using Packet.Enums;
using Packet.Interfaces.Server;

namespace Packet.Server
{
    /// <summary>
    /// Represents a HTTP request for HTTP/1.0 and newer.
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
        /// Initializes a new instance of a HTTP request for HTTP/1.0 and newer.
        /// </summary>
        /// <param name="method">The request method.</param>
        /// <param name="version">The HTTP version of the request.</param>
        public FullHttpRequest(HttpMethod method, IHttpVersion version) 
            : base(method, version)
        {
        }
    }
}
