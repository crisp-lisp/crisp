using System.Collections.Generic;

namespace Packet.Interfaces.Server
{
    /// <summary>
    /// Represents a request conforming to HTTP/1.0 or newer.
    /// </summary>
    public interface IFullHttpRequest : IHttpRequest
    {
        /// <summary>
        /// Gets or sets a dictionary of headers in the request.
        /// </summary>
        Dictionary<string, string> Headers { get; set; }

        /// <summary>
        /// Gets or sets the request body.
        /// </summary>
        byte[] RequestBody { get; set; }
    }
}
