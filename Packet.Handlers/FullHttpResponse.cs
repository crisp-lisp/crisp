using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;

using Packet.Interfaces.Server;

namespace Packet.Handlers
{
    /// <summary>
    /// Represents a HTTP response for HTTP/1.0 and newer.
    /// </summary>
    public class FullHttpResponse : HttpResponse
    {
        /// <summary>
        /// Gets or sets the status code contained in the response.
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the headers contained in the response.
        /// </summary>
        public Dictionary<string, string> Headers { get; set; }

        /// <summary>
        /// Initializes a new instance of a HTTP response for HTTP/1.0 and newer.
        /// </summary>
        /// <param name="version">The HTTP version of the response.</param>
        public FullHttpResponse(IHttpVersion version) : base(version)
        {
            Headers = new Dictionary<string, string>();
        }

        public override void WriteTo(TcpClient socket)
        {
            var dataOutputStream = new BufferedStream(socket.GetStream());
            var textOutputStream = new StreamWriter(dataOutputStream);

            // Write response line.
            textOutputStream.WriteLine($"{Version} {StatusCode} {ReasonPhraseHelper.TryGet(StatusCode, "Unknown")}");

            // Write headers.
            foreach (var header in Headers)
            {
                textOutputStream.WriteLine($"{header.Key}: {header.Value}");
            }
            textOutputStream.WriteLine();
            textOutputStream.Flush();

            // Write body if we have one.
            if (Content.Length > 0)
            {
                dataOutputStream.Write(Content, 0, Content.Length);
                dataOutputStream.Flush();
            }
        }
    }
}
