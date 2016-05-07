using System.IO;

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
        /// <param name="stream">The network stream to read from.</param>
        /// <returns></returns>
        byte[] GetData(Stream stream);
    }
}
