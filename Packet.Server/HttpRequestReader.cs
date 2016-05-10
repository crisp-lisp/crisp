using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Web;

using Packet.Interfaces.Configuration;
using Packet.Interfaces.Server;

namespace Packet.Server
{
    public class HttpRequestReader : IHttpRequestReader
    {
        private readonly IHttpRequestParser _httpRequestParser;
        private readonly IPacketConfiguration _packetConfiguration;

        /// <summary>
        /// Initializes a new instance of a HTTP request reader used to read HTTP requests from a stream as raw bytes.
        /// </summary>
        /// <param name="httpRequestParser"></param>
        /// <param name="packetConfigurationProvider">The configuration provider service.</param>
        public HttpRequestReader(
            IHttpRequestParser httpRequestParser,
            IPacketConfigurationProvider packetConfigurationProvider)
        {
            _httpRequestParser = httpRequestParser;
            _packetConfiguration = packetConfigurationProvider.Get();
        }

        /// <summary>
        /// Gets the content length from a set of HTTP headers, or returns 0 if the header is absent.
        /// </summary>
        /// <param name="headers">The set of headers to get the content length from.</param>
        /// <returns></returns>
        private static int GetContentLength(IReadOnlyDictionary<string, string> headers)
        {
            const string key = "Content-Length";
            return headers.ContainsKey(key) ? int.Parse(headers[key]) : 0;
        }

        public byte[] Read(TcpClient socket)
        {
            using (var outputStream = new MemoryStream())
            using (var outputWriter = new StreamWriter(outputStream))
            {
                var inputStream = new BufferedStream(socket.GetStream());

                // Get version from request line.
                var requestLine = inputStream.ReadLine(true);
                var version = _httpRequestParser.GetVersion(requestLine);
                
                // Bad header line.
                if (version == null)
                {
                    throw new HttpException("The HTTP header line was invalid.");
                }

                // Emit request line.
                outputWriter.Write(requestLine);

                // For HTTP/0.9 requests, this is the only line.
                if (version.Major == 0 && version.Minor == 9)
                { 
                    return outputStream.ToArray();
                }

                // Read in headers.
                var headers = new Dictionary<string, string>();
                string headerBuffer;
                while ((headerBuffer = inputStream.ReadLine()) != null)
                {
                    // Blank line means end of headers.
                    if (headerBuffer == string.Empty)
                        break;

                    // Add to headers.
                    var splitHeader = headerBuffer.Split(':');
                    headers.Add(splitHeader[0], string.Join(":", splitHeader.Skip(1)).Trim());
                }
                
                // Emit headers.
                foreach (var header in headers)
                {
                    outputWriter.WriteLine($"{header.Key}: {header.Value}");
                }

                // Emit blank line to separate headers from body.
                outputWriter.WriteLine();

                // Flush, we're done with this.
                outputWriter.Flush();

                // Enforce post length cap.
                var contentLength = GetContentLength(headers);
                if (contentLength > _packetConfiguration.MaxPostSize)
                {
                    throw new HttpException("Post length is larger than the maximum of " +
                                            $"{_packetConfiguration.MaxPostSize} bytes.");
                }

                // Read post data into memory buffer.
                var buffer = new byte[4096]; 
                var remainingLength = contentLength;
                while (remainingLength > 0)
                {
                    var numRead = inputStream.Read(buffer, 0, Math.Min(4096, remainingLength));
                    if (numRead == 0)
                    {
                        if (remainingLength == 0)
                        {
                            break;
                        }

                        throw new HttpException("Client disconnected during socket read.");
                    }
                    remainingLength -= numRead;
                    outputStream.Write(buffer, 0, numRead);
                }

                outputStream.Flush();
                return outputStream.ToArray();
            }
        }
    }
}
