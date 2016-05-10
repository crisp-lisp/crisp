using System.Net.Sockets;
using System.Threading;

using Packet.Interfaces.Logging;
using Packet.Interfaces.Server;

namespace Packet.Server
{
    /// <summary>
    /// Represents a HTTP connection handler that uses multi-threading.
    /// </summary>
    public class ThreadedHttpConnectionHandler : IHttpConnectionHandler
    {
        private readonly IHttpRequestReader _httpRequestReader;

        private readonly IHttpRequestParser _httpRequestParser;

        private readonly IHttpRequestHandler _httpRequestHandler;

        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of a HTTP connection handler that uses multi-threading.
        /// </summary>
        /// <param name="httpRequestReader">The HTTP request reader service.</param>
        /// <param name="httpRequestParser">The HTTP request parser service.</param>
        /// <param name="httpRequestHandler">The HTTP request handler service.</param>
        /// <param name="logger">The logger that should be used to log server events.</param>
        public ThreadedHttpConnectionHandler(
            IHttpRequestReader httpRequestReader, 
            IHttpRequestParser httpRequestParser,
            IHttpRequestHandler httpRequestHandler,
            ILogger logger)
        {
            _httpRequestReader = httpRequestReader;
            _httpRequestParser = httpRequestParser;
            _httpRequestHandler = httpRequestHandler;
            _logger = logger;
        }

        private void HandleOnThread(object obj)
        {
            var client = (TcpClient) obj;

            // Read HTTP request.
            var data = _httpRequestReader.Read(client);
            _logger.WriteLine($"Read {data.Length} bytes from client.");

            // Parse request.
            var request = _httpRequestParser.Parse(data);
            _logger.WriteLine($"Request uses {request.Version}.");

            // Formulate response and write to output.
            var response = _httpRequestHandler.Handle(request);
            response.WriteTo(client);

            // Close connection.
            client.Close();
            _logger.Write("Finshed dealing with request.");
        }

        public void Handle(TcpClient client)
        {
            // How many threads left to work with?
            int x;
            int y;
            ThreadPool.GetAvailableThreads(out x, out y);

            // Log threads available.
            _logger.WriteLine($"{x} worker threads available to deal with request.");

            ThreadPool.QueueUserWorkItem(HandleOnThread, client); // Queue thread.
        }
    }
}
