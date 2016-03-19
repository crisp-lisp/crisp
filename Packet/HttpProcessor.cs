using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Packet
{
    public class HttpProcessor
    {
        private readonly TcpClient _socket;
        private readonly HttpServer _server;

        private Stream _inputStream;
        private string _httpMethod;
        private string _httpProtocolVersionString;
        private Hashtable _httpHeaders = new Hashtable();

        private static int MAX_POST_SIZE = 10*1024*1024;

        public StreamWriter OutputStream { get; private set; }

        public string HttpUrl { get; private set; }

        public HttpProcessor(TcpClient socket, HttpServer server)
        {
            _socket = socket;
            _server = server;
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
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                WriteFailure();
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
                _httpHeaders[name] = value;
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
            if (_httpHeaders.ContainsKey("Content-Length"))
            {
                content_len = Convert.ToInt32(_httpHeaders["Content-Length"]);
                if (content_len > MAX_POST_SIZE)
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

        public void WriteSuccess(string contentType = "text/html")
        {
            OutputStream.WriteLine("HTTP/1.0 200 OK");
            OutputStream.WriteLine("Content-Type: " + contentType);
            OutputStream.WriteLine("Connection: close");
            OutputStream.WriteLine("");
        }

        public void WriteFailure()
        {
            OutputStream.WriteLine("HTTP/1.0 404 File not found");
            OutputStream.WriteLine("Connection: close");
            OutputStream.WriteLine("");
        }
    }
}
