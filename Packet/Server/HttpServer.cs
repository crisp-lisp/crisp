using System;
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

        protected ILogger _logger;

        /// <summary>
        /// Gets whether or not this server is currently actively listening for requests.
        /// </summary>
        protected bool IsActive { get; private set; }

        /// <summary>
        /// Gets the port number that this server is currently listening on.
        /// </summary>
        protected int Port { get; }

        /// <summary>
        /// Gets the IP address that this server is bound to.
        /// </summary>
        protected string IpAddress { get; }

        /// <summary>
        /// Initializes a new instance of an abstract HTTP server.
        /// </summary>
        /// <param name="ipAddress">The IP address that this server should be bound to.</param>
        /// <param name="port">The port that the server should listen for requests on.</param>
        /// <param name="logger">The logger that should be used to log server events.</param>
        protected HttpServer(string ipAddress, int port, ILogger logger)
        {
            Port = port;
            IpAddress = ipAddress;
            _logger = logger;
        }
        
        public void Listen()
        {
            // Start TCP listener.
            var ipAddress = IPAddress.Parse(IpAddress);
            _listener = new TcpListener(ipAddress, Port);
            _listener.Start();
            IsActive = true;

            // While we're actively listening for connections.
            while (IsActive) // TODO: Weird way to listen for multiple requests.
            {
                try
                {
                    // Pass request to processor and process in a new thread.
                    var client = _listener.AcceptTcpClient();
                    var processor = new HttpProcessor(client, this, _logger);
                    var thread = new Thread(processor.Process);
                    thread.Start();
                }
                catch (Exception ex)
                {
                    _logger.WriteError(ex);
                }
            }
        }

        public abstract void HandleGetRequest(HttpProcessor processor);

        public abstract void HandlePostRequest(HttpProcessor processor, StreamReader inputStream);
    }
}
