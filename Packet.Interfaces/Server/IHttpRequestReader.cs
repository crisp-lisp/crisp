using System.IO;
using System.Net.Sockets;

namespace Packet.Interfaces.Server
{
    /// <summary>
    /// Represents a HTTP request reader used to read HTTP requests from a stream as raw bytes.
    /// </summary>
    public interface IHttpRequestReader
    {
        /// <summary>
        /// Gets the HTTP requests from a given stream as raw bytes.
        /// </summary>
        /// <param name="socket">The TCP client to read from.</param>
        /// <returns></returns>
        byte[] Read(TcpClient socket);
    }
}
