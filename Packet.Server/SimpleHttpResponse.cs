using System.IO;
using System.Net.Sockets;

namespace Packet.Server
{
    public class SimpleHttpResponse : HttpResponse
    {
        public SimpleHttpResponse(byte[] content) : base(new HttpVersion(0, 9))
        {
            Content = content;
        }

        public override void WriteTo(TcpClient socket)
        {
            socket.GetStream().Write(Content, 0, Content.Length); // Just dump response content.
        }
    }
}
