﻿using System.Net;
using System.Net.Sockets;

using Packet.Interfaces.Configuration;
using Packet.Interfaces.Logging;
using Packet.Interfaces.Server;

namespace Packet.Server
{
    public class PacketHttpServer : IHttpServer
    {
        private TcpListener _listener;

        private readonly ILogger _logger;

        private readonly IPacketConfiguration _packetConfiguration;

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
        /// <param name="packetConfigurationProvider"></param>
        /// <param name="logger">The logger that should be used to log server events.</param>
        /// <param name="httpRequestParser"></param>
        /// <param name="httpRequestReader"></param>
        /// <param name="httpRequestHandler"></param>
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
            _listener = new TcpListener(ipAddress, _packetConfiguration.Port);
            _listener.Start();
            IsActive = true;

            _logger.WriteLine($"Bound to IP {ipAddress} and listening on port {_packetConfiguration.Port}...");

            // While we're actively listening for connections.
            while (IsActive)
            {
                _logger.WriteLine("Waiting for a request...");

                // Wait for TCP client connect.
                var client = _listener.AcceptTcpClient();

                _logger.WriteLine("Listener accepted TCP client...");
                
                // Read HTTP request.
                var data = _httpRequestReader.Read(client);

                _logger.WriteLine($"Read {data.Length} bytes from client.");

                // Parse request.
                var request = _httpRequestParser.Parse(data);

                _logger.WriteLine($"Request uses {request.Version}.");

                // Formulate response.
                var response = _httpRequestHandler.Handle(request);

                // Write response to output.
                response.WriteTo(client);
               
                client.Close();
            }
        }
    }
}