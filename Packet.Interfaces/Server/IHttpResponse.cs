using System;
using System.Net.Sockets;

namespace Packet.Interfaces.Server
{
    /// <summary>
    /// Represents a response to a HTTP request.
    /// </summary>
    public interface IHttpResponse
    {
        /// <summary>
        /// Gets the version of HTTP this response uses.
        /// </summary>
        IHttpVersion Version { get; }

        /// <summary>
        /// Gets the body of this response.
        /// </summary>
        byte[] Content { get; set; }

        /// <summary>
        /// Writes the response to a socket.
        /// </summary>
        /// <param name="socket">The socket to write to.</param>
        void WriteTo(TcpClient socket);
    }
}
