using Packet.Enums;

namespace Packet.Interfaces.Server
{
    /// <summary>
    /// Represents a HTTP request made to the server.
    /// </summary>
    public interface IHttpRequest
    {
        /// <summary>
        /// Gets the HTTP method (verb) contained in the request.
        /// </summary>
        HttpMethod Method { get; }

        /// <summary>
        /// Gets the HTTP version of the request.
        /// </summary>
        IHttpVersion Version { get; }

        /// <summary>
        /// Gets the URL of the resource requested.
        /// </summary>
        string Url { get; }
    }
}