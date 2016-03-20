using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Packet.Server
{
    /// <summary>
    /// Represents an abstract HTTP server.
    /// </summary>
    internal abstract class HttpServer : IHttpServer
    {
        private TcpListener _listener;
        
        /// <summary>
        /// Gets whether or not this server is currently actively listening for requests.
        /// </summary>
        protected bool IsActive { get; private set; }

        /// <summary>
        /// Gets the port number that this server is currently listening on.
        /// </summary>
        protected int Port { get; }

        /// <summary>
        /// Initializes a new instance of an abstract HTTP server.
        /// </summary>
        /// <param name="port">The port that the server should listen for requests on.</param>
        protected HttpServer(int port)
        {
            Port = port;
        }
        
        public void Listen()
        {
            // Start TCP listener.
            var ipAddress = Dns.GetHostEntry("localhost").AddressList[0];
            _listener = new TcpListener(ipAddress, Port);
            _listener.Start();
            IsActive = true;

            // While we're actively listening for connections.
            while (IsActive) // TODO: Weird way to listen for multiple requests.
            {
                // Pass request to processor and process in a new thread.
                var client = _listener.AcceptTcpClient();
                var processor = new HttpProcessor(client, this);
                var thread = new Thread(processor.Process);
                thread.Start();
            }
        }

        public abstract void HandleGetRequest(HttpProcessor processor);

        public abstract void HandlePostRequest(HttpProcessor processor, StreamReader inputStream);
    }
}
