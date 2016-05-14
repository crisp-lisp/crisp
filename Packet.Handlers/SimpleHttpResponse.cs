using System.Net.Sockets;

namespace Packet.Handlers
{
    /// <summary>
    /// Represents a HTTP/0.9 simple-format response.
    /// </summary>
    public class SimpleHttpResponse : HttpResponse
    {
        /// <summary>
        /// Initializes a new instance of a HTTP/0.9 simple-format response.
        /// </summary>
        /// <param name="content">The content of the response.</param>
        public SimpleHttpResponse(byte[] content) : base(new HttpVersion(0, 9))
        {
            Content = content;
        }

        public override void WriteTo(TcpClient socket)
        {
            // Just dump response content.
            socket.GetStream().Write(Content, 0, Content.Length); 
        }
    }
}
