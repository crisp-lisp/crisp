using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using Packet.Interfaces.Server;

namespace Packet.Server
{
    public class FullHttpResponse : HttpResponse
    {
        public int StatusCode { get; set; }

        public Dictionary<string, string> Headers { get; set; } 

        public FullHttpResponse(IHttpVersion version) : base(version)
        {
            Headers = new Dictionary<string, string>();
        }

        public override void WriteTo(TcpClient socket)
        {
            var dataOutputStream = new BufferedStream(socket.GetStream());
            var textOutputStream = new StreamWriter(dataOutputStream);

            textOutputStream.WriteLine($"{Version} {StatusCode} {ReasonPhrase.TryGet(StatusCode, "Unknown")}");

            foreach (var header in Headers)
            {
                textOutputStream.WriteLine($"{header.Key}: {header.Value}");
            }
            textOutputStream.WriteLine();
            textOutputStream.Flush();

            if (Content.Length > 0)
            {
                dataOutputStream.Write(Content, 0, Content.Length);
                dataOutputStream.Flush();
            }
        }
    }
}
