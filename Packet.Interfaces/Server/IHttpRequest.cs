using Packet.Enums;

namespace Packet.Interfaces.Server
{
    /// <summary>
    /// Represents a HTTP request made to the server.
    /// </summary>
    public interface IHttpRequest
    {
        /// <summary>
        /// The type of the request.
        /// </summary>
        RequestType RequestType { get; }

        /// <summary>
        /// Gets the HTTP method (verb) contained in the request.
        /// </summary>
        HttpMethod Method { get; }

        /// <summary>
        /// Gets the URL of the resource requested.
        /// </summary>
        string Url { get; }
    }
}