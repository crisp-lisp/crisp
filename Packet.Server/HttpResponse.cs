using System.Net.Sockets;

using Packet.Interfaces.Server;

namespace Packet.Server
{
    public abstract class HttpResponse : IHttpResponse
    {
        public IHttpVersion Version { get; }

        public byte[] Content { get; set; }

        /// <summary>
        /// Initializes a new instance of a response to a HTTP request.
        /// </summary>
        /// <param name="version">The HTTP version of the response.</param>
        protected HttpResponse(IHttpVersion version)
        {
            Version = version;
            Content = new byte[0];
        }

        public abstract void WriteTo(TcpClient socket);
    }
}
