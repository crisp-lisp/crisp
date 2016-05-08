using System.IO;
using System.Net.Sockets;
using Packet.Interfaces.Server;

namespace Packet.Server
{
    public abstract class HttpResponse : IHttpResponse
    {
        public IHttpVersion Version { get; }

        public byte[] Content { get; set; }
        
        protected HttpResponse(IHttpVersion version)
        {
            Version = version;
            Content = new byte[0];
        }

        public abstract void WriteTo(TcpClient socket);
    }
}
