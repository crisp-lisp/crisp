using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

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

        private readonly TcpClient _socket;

        private readonly HttpServer _server;

        private Stream _inputStream;

        private string _httpMethod;

        private string _httpProtocolVersionString;

        /// <summary>
        /// Gets a collection of HTTP headers passed up by the client.
        /// </summary>
        public Dictionary<string, string> Headers { get; }

        /// <summary>
        /// Gets the <see cref="StreamWriter"/> used to write a response to the client.
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
            _socket = socket;
            _server = server;
            Headers = new Dictionary<string, string>();
        }

        /// <summary>
        /// Gets the reason phrase for a HTTP status code.
        /// </summary>
        /// <param name="statusCode">The HTTP status code.</param>
        /// <returns></returns>
        private static string GetReasonPhrase(int statusCode)
        {
            return new Dictionary<int, string>
            {
                {200, "OK"},
                {201, "CREATED"},
                {202, "ACCEPTED"},
                {204, "No Content"},
                {301, "Moved Permanently"},
                {302, "Moved Temporarily"},
                {304, "Not Modified"},
                {400, "Bad Request"},
                {401, "Unauthorized"},
                {403, "Forbidden"},
                {404, "Not Found"},
                {500, "Internal Server Error"},
                {501, "Not Implemented"},
                {502, "Bad Gateway"},
                {503, "Service Unavailable"}
            }[statusCode];
        }

        private static string StreamReadLine(Stream inputStream)
        {
            var data = new StringBuilder();

            int buffer;
            while ((buffer = inputStream.ReadByte()) != '\n')
            {
                switch (buffer)
                {
                    case '\r':
                        break;
                    case -1:
                        Thread.Sleep(1);
                        break;
                    default:
                        data.Append(Convert.ToChar(buffer));
                        break;
                }
            }

            return data.ToString();
        }

        public void Process()
        {
            _inputStream = new BufferedStream(_socket.GetStream());
            OutputStream = new StreamWriter(new BufferedStream(_socket.GetStream()));

            try
            {
                ParseRequest();
                ReadHeaders();
                switch (_httpMethod)
                {
                    case "GET":
                        HandleGetRequest();
                        break;
                    case "POST":
                        HandlePostRequest();
                        break;
                }
            }
            catch
            {
                // We consider a failure here to be a bad request.
                WriteResponse(400, "text/html");
                OutputStream.Write(Properties.Resources.DefaultBadRequestPage);
            }

            OutputStream.Flush();
            _socket.Close();
        }

        private void ParseRequest()
        {
            var request = StreamReadLine(_inputStream);
            var tokens = request.Split(' ');
            if (tokens.Length != 3)
            {
                throw new Exception("HTTP request line was invalid.");
            }
            _httpMethod = tokens[0].ToUpper();
            HttpUrl = tokens[1];
            _httpProtocolVersionString = tokens[2];

            Console.WriteLine($"Starting: {request}");
        }

        private void ReadHeaders()
        {
            Console.WriteLine("ReadHeaders()");
            string line;
            while ((line = StreamReadLine(_inputStream)) != null)
            {
                if (line == "")
                {
                    Console.WriteLine("Got headers.");
                    return;
                }

                int separator = line.IndexOf(":");
                if (separator == -1)
                {
                    throw new Exception($"Invalid HTTP header line: {line}");
                }
                string name = line.Substring(0, separator);
                int pos = separator + 1;
                while ((pos < line.Length) && (line[pos] == ' '))
                {
                    pos++;
                }
                string value = line.Substring(pos, line.Length - pos);
                Console.WriteLine("header: {0}:{1}", name, value);
                Headers.Add(name, value);
            }
        }

        private void HandleGetRequest()
        {
            _server.HandleGetRequest(this);
        }

        private void HandlePostRequest()
        {
            Console.Write("Get post data start.");
            int content_len = 0;
            var ms = new MemoryStream();
            if (Headers.ContainsKey("Content-Length"))
            {
                content_len = Convert.ToInt32(Headers["Content-Length"]);
                if (content_len > MaxPostSize)
                {
                    throw new Exception("Post length too big!");
                }
                byte[] buf = new byte[4096];
                int to_read = content_len;
                while (to_read > 0)
                {
                    Console.Write("Starting read.");
                    int numread = this._inputStream.Read(buf, 0, Math.Min(4096, to_read));
                    if (numread == 0)
                    {
                        if (to_read == 0)
                        {
                            break;
                        }
                        else
                        {
                            throw new Exception("Client disconnect");
                        }
                    }
                    to_read -= numread;
                    ms.Write(buf, 0, numread);
                }
                ms.Seek(0, SeekOrigin.Begin);
            }
            Console.WriteLine("Get post data end");
            _server.HandlePostRequest(this, new StreamReader(ms));
        }

        /// <summary>
        /// Writes a set of response headers to the output stream.
        /// </summary>
        /// <param name="statusCode">The status code of the response.</param>
        /// <param name="contentType">The content type of the response.</param>
        /// <param name="headers">Any additional headers to include in the response.</param>
        public void WriteResponse(int statusCode, string contentType, Dictionary<string, string> headers)
        {
            OutputStream.WriteLine($"HTTP/1.0 {statusCode} {GetReasonPhrase(statusCode)}");
            OutputStream.WriteLine("Content-Type: " + contentType);
            OutputStream.WriteLine("Connection: close");
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
