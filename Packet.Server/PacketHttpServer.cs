using System.Net;
using System.Net.Sockets;

using Packet.Interfaces.Configuration;
using Packet.Interfaces.Logging;
using Packet.Interfaces.Server;

namespace Packet.Server
{
    /// <summary>
    /// Represents a Packet HTTP server.
    /// </summary>
    public class PacketHttpServer : IHttpServer
    {
        private readonly IPacketConfiguration _packetConfiguration;

        private readonly ILogger _logger;

        private readonly IHttpRequestParser _httpRequestParser;

        private readonly IHttpRequestReader _httpRequestReader;

        private readonly IHttpRequestHandler _httpRequestHandler;

        /// <summary>
        /// Gets whether or not this server is currently actively listening for requests.
        /// </summary>
        protected bool IsActive { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="packetConfigurationProvider">The server configuration provider service.</param>
        /// <param name="logger">The logger that should be used to log server events.</param>
        /// <param name="httpRequestParser">The HTTP request parser service.</param>
        /// <param name="httpRequestReader">The HTTP request reader service.</param>
        /// <param name="httpRequestHandler">The HTTP request handler service.</param>
        public PacketHttpServer(
            IPacketConfigurationProvider packetConfigurationProvider, 
            ILogger logger, 
            IHttpRequestParser httpRequestParser, 
            IHttpRequestReader httpRequestReader,
            IHttpRequestHandler httpRequestHandler)
        {
            _packetConfiguration = packetConfigurationProvider.Get();
            _logger = logger;
            _httpRequestParser = httpRequestParser;
            _httpRequestReader = httpRequestReader;
            _httpRequestHandler = httpRequestHandler;
        }

        public void Listen()
        {
            // Start TCP listener.
            var ipAddress = IPAddress.Parse(_packetConfiguration.BindingIpAddress);
            var listener = new TcpListener(ipAddress, _packetConfiguration.Port);
            listener.Start();
            IsActive = true;

            _logger.WriteLine($"Bound to IP {ipAddress} and listening on port {_packetConfiguration.Port}...");

            // While we're actively listening for connections.
            while (IsActive)
            {
                _logger.WriteLine("Waiting for a request...");

                // Wait for TCP client connect.
                var client = listener.AcceptTcpClient();

                _logger.WriteLine("Listener accepted TCP client...");
                
                // Read HTTP request.
                var data = _httpRequestReader.Read(client);

                _logger.WriteLine($"Read {data.Length} bytes from client.");

                // Parse request.
                var request = _httpRequestParser.Parse(data);

                _logger.WriteLine($"Request uses {request.Version}.");

                // Formulate response and write to output.
                var response = _httpRequestHandler.Handle(request);
                response.WriteTo(client);

                _logger.Write("Finshed dealing with request.");
               
                client.Close();
            }
        }
    }
}
