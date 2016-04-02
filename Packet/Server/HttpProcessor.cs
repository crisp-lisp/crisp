using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Web;

namespace Packet.Server
{
    /// <summary>
    /// An implementation of a HTTP/1.0 request processor.
    /// </summary>
    internal class HttpProcessor
    {
        /// <summary>
        /// Gets the maximum allowed size of a POST request body.
        /// </summary>
        private const int MaxPostSize = 10*1024*1024;

        /// <summary>
        /// Gets the socket open to the client.
        /// </summary>
        public TcpClient Socket { get; }

        /// <summary>
        /// Gets the server coupled with this object for request handling.
        /// </summary>
        public HttpServer Server { get; }

        /// <summary>
        /// Gets the input stream for reading data passed up by the client.
        /// </summary>
        public Stream InputStream { get; private set; }

        /// <summary>
        /// Gets the HTTP method (verb) passed up by the client.
        /// </summary>
        public string HttpMethod { get; private set; }

        /// <summary>
        /// Gets the HTTP protocol version string passed up by the client.
        /// </summary>
        public string HttpProtocolVersionString { get; private set; }

        /// <summary>
        /// Gets a collection of HTTP headers passed up by the client.
        /// </summary>
        public Dictionary<string, string> Headers { get; }

        /// <summary>
        /// Gets the output stream used to write a response to the client.
        /// </summary>
        public StreamWriter OutputStream { get; private set; }

        /// <summary>
        /// Gets the URL passed up to the server as part of the request.
        /// </summary>
        public string HttpUrl { get; private set; }

        /// <summary>
        /// Initializes a new instance of of a HTTP/1.0 request processor.
        /// </summary>
        /// <param name="socket">The socket to write to.</param>
        /// <param name="server">The server that spawned this processor.</param>
        public HttpProcessor(TcpClient socket, HttpServer server)
        {
            Socket = socket;
            Server = server;
            Headers = new Dictionary<string, string>();
        }

        /// <summary>
        /// Reads a line of text from a <see cref="Stream"/> instance.
        /// </summary>
        /// <param name="inputStream">The stream to read from.</param>
        /// <returns></returns>
        private static string StreamReadLine(Stream inputStream)
        {
            var line = new StringBuilder();

            // Read until newline.
            int buffer;
            while ((buffer = inputStream.ReadByte()) != '\n')
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

        public void Process()
        {
            /*
             * Can't use a StreamReader here because we might need to read binary data passed up by the client in a
             * POST request.
             */
            InputStream = new BufferedStream(Socket.GetStream());
            OutputStream = new StreamWriter(new BufferedStream(Socket.GetStream()));

            try
            {
                ParseRequest();
                ReadHeaders();
                switch (HttpMethod)
                {
                    case "GET":
                        HandleGetRequest();
                        break;
                    case "POST":
                        HandlePostRequest();
                        break;
                    default:
                        throw new HttpException($"The HTTP verb {HttpMethod} is not supported.");
                }
            }
            catch
            {
                // We consider a failure here to be a bad request.
                WriteResponse(400, "text/html");
                OutputStream.Write(Properties.Resources.DefaultBadRequestPage);
            }

            OutputStream.Flush();
            Socket.Close();
        }

        /// <summary>
        /// Parses the request line passed up the client.
        /// </summary>
        private void ParseRequest()
        {
            var request = StreamReadLine(InputStream);
            var tokens = request.Split(' ');
            if (tokens.Length != 3)
            {
                throw new HttpException("HTTP request line was invalid."); // Bad request line.
            }
            HttpMethod = tokens[0].ToUpper();
            HttpUrl = tokens[1];
            HttpProtocolVersionString = tokens[2];
        }

        /// <summary>
        /// Parses all header lines passed up by the client.
        /// </summary>
        private void ReadHeaders()
        {
            string line;
            while ((line = StreamReadLine(InputStream)) != null)
            {
                if (line == string.Empty)
                {
                    break; // Blank line means end of headers.
                }

                if (!line.Contains(':')) 
                {
                    throw new HttpException($"Invalid HTTP header line: {line}"); // Bad header line.
                }

                // Add to headers.
                var split = line.Split(':');
                Headers.Add(split[0], string.Join(":", split.Skip(1)).Trim());
            }
        }

        /// <summary>
        /// Delegates handling of a get request to the coupled server.
        /// </summary>
        private void HandleGetRequest()
        {
            Server.HandleGetRequest(this);
        }

        /// <summary>
        /// Delegates handling of a post request to the coupled server.
        /// </summary>
        private void HandlePostRequest()
        {
            var stream = new MemoryStream();
            if (Headers.ContainsKey("Content-Length"))
            {
                // Enforce post length cap.
                var contentLength = Convert.ToInt32(Headers["Content-Length"]);
                if (contentLength > MaxPostSize)
                {
                    throw new HttpException($"Post length is larger than the maximum of {MaxPostSize} bytes.");
                }

                // Read post data into memory buffer.
                var buffer = new byte[4096];
                var remainingLength = contentLength;
                while (remainingLength > 0)
                {
                    var numRead = InputStream.Read(buffer, 0, Math.Min(4096, remainingLength));
                    if (numRead == 0)
                    {
                        if (remainingLength == 0)
                        {
                            break;
                        }
                        throw new HttpException("Client disconnected during socket read.");
                    }
                    remainingLength -= numRead;
                    stream.Write(buffer, 0, numRead);
                }
                stream.Seek(0, SeekOrigin.Begin);
            }
            Server.HandlePostRequest(this, new StreamReader(stream));
        }

        /// <summary>
        /// Writes a set of response headers to the output stream.
        /// </summary>
        /// <param name="statusCode">The status code of the response.</param>
        /// <param name="contentType">The content type of the response.</param>
        /// <param name="headers">Any additional headers to include in the response.</param>
        public void WriteResponse(int statusCode, string contentType, Dictionary<string, string> headers)
        {
            OutputStream.WriteLine($"HTTP/1.0 {statusCode} {ReasonPhrase.TryGet(statusCode, "Unknown")}");
            OutputStream.WriteLine("Content-Type: " + contentType);
            OutputStream.WriteLine("Connection: close");

            // Any custom headers.
            foreach (var entry in headers) 
            {
                OutputStream.WriteLine(entry.Key + ": " + entry.Value);
            }

            OutputStream.WriteLine("");
        }

        /// <summary>
        /// Writes a set of response headers to the output stream.
        /// </summary>
        /// <param name="statusCode">The status code of the response.</param>
        /// <param name="contentType">The content type of the response.</param>
        public void WriteResponse(int statusCode, string contentType)
        {
            WriteResponse(statusCode, contentType, new Dictionary<string, string>());
        }
    }
}
