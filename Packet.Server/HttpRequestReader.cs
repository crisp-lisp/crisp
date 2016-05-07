using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Packet.Interfaces.Configuration;
using Packet.Interfaces.Server;

namespace Packet.Server
{
    public class HttpRequestReader : IHttpRequestReader
    {
        private readonly IPacketConfiguration _packetConfiguration;

        public HttpRequestReader(IPacketConfigurationProvider packetConfigurationProvider)
        {
            _packetConfiguration = packetConfigurationProvider.Get();
        }

        public byte[] GetData(Stream stream)
        {
            var dataStream = new MemoryStream();

            var inputStream = new BufferedStream(stream);
            var requestLine = StreamReadLine(inputStream);

            // Check request line against simple.

            var headers = new Dictionary<string, string>();

            string line;
            while ((line = StreamReadLine(inputStream)) != null)
            {
                if (line == string.Empty)
                {
                    break; // Blank line means end of headers.
                }

                // Add to headers.
                var split = line.Split(':');
                headers.Add(split[0], string.Join(":", split.Skip(1)).Trim());
            }

            var contentLength = headers.ContainsKey("Content-Length") ? int.Parse(headers["Content-Length"]) : 0;

            using (var headerWriter = new StreamWriter(dataStream))
            {
                headerWriter.WriteLine(requestLine);
                foreach (var header in headers)
                {
                    headerWriter.WriteLine($"{header.Key}: {header.Value}");
                }
                headerWriter.WriteLine();
            }

            // Enforce post length cap.
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
                    // TODO: Throw it! Client disconnected!
                }
                remainingLength -= numRead;
                dataStream.Write(buffer, 0, numRead);
            }
            
            return dataStream.ToArray();
        }


        /// <summary>
        /// Reads a line of text from a <see cref="Stream"/> instance.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <returns></returns>
        private static string StreamReadLine(Stream stream)
        {
            var line = new StringBuilder();

            // Read until newline.
            int buffer;
            while ((buffer = stream.ReadByte()) != '\n')
            {
                switch (buffer)
                {
                    case '\r': // Leave out carriage return char.
                    case -1:
                        break;
                    default:
                        line.Append(Convert.ToChar(buffer));
                        break;
                }
            }

            return line.ToString();
        }
    }
}
