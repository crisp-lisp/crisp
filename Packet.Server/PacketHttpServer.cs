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

        private readonly IHttpConnectionHandler _httpConnectionHandler;

        /// <summary>
        /// Gets whether or not this server is currently actively listening for requests.
        /// </summary>
        protected bool IsActive { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="packetConfigurationProvider">The server configuration provider service.</param>
        /// <param name="logger">The logger that should be used to log server events.</param>
        /// <param name="httpConnectionHandler"></param>
        public PacketHttpServer(
            IPacketConfigurationProvider packetConfigurationProvider, 
            ILogger logger, 
            IHttpConnectionHandler httpConnectionHandler)
        {
            _packetConfiguration = packetConfigurationProvider.Get();
            _logger = logger;
            _httpConnectionHandler = httpConnectionHandler;
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

                // Deal with connections as and when they're made.
                _httpConnectionHandler.Handle(listener.AcceptTcpClient());

                _logger.WriteLine("Thread dispatched to handle request.");
            }
        }
    }
}
