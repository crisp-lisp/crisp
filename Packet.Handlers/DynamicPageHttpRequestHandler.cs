using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Crisp.IoC;

using Packet.Interfaces.Configuration;
using Packet.Interfaces.Logging;
using Packet.Interfaces.Server;

namespace Packet.Handlers
{
    /// <summary>
    /// Represents a HTTP request handler for serving dynamic pages.
    /// </summary>
    public class DynamicPageHttpRequestHandler : HttpRequestHandler
    {
        private readonly IPacketConfiguration _packetConfiguration;

        private readonly IUrlResolver _urlResolver;

        private readonly IErrorPageContentRetriever _errorPageContentRetriever;

        private readonly IHttpRequestConverter _httpRequestConverter;

        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of a HTTP request handler for serving dynamic pages.
        /// </summary>
        /// <param name="packetConfigurationProvider">The server configuration provider service.</param>
        /// <param name="urlResolver">The URL resolution service.</param>
        /// <param name="errorPageContentRetriever">The error page content retrieval service.</param>
        /// <param name="httpRequestConverter">The HTTP request conversion service.</param>
        /// <param name="logger">The logger to use to log server events.</param>
        public DynamicPageHttpRequestHandler(
            IPacketConfigurationProvider packetConfigurationProvider,
            IUrlResolver urlResolver,
            IErrorPageContentRetriever errorPageContentRetriever,
            IHttpRequestConverter httpRequestConverter,
            ILogger logger)
        {
            _packetConfiguration = packetConfigurationProvider.Get(); 
            _urlResolver = urlResolver;
            _errorPageContentRetriever = errorPageContentRetriever;
            _httpRequestConverter = httpRequestConverter;
            _logger = logger;
        }

        /// <summary>
        /// Returns true if the given file extension if configured to be interpreted.
        /// </summary>
        /// <param name="extension">The file extension to check, including the leading dot.</param>
        /// <returns></returns>
        private bool IsInterpretedFileExtension(string extension)
        {
            return _packetConfiguration.CrispFileExtensions.Contains(extension);
        }

        protected override IHttpResponse AttemptHandle(IHttpRequest request)
        {
            var resolvedPath = _urlResolver.Resolve(request.Url); // Resolve URL.

            // If not dynamic page, defer.
            if (!IsInterpretedFileExtension(Path.GetExtension(resolvedPath)))
            {
                return null;
            }
            
            // Evaluate webpage.
            IHttpResponse response;
            try
            {
                // Create runtime for file and run it.
                var runtime = CrispRuntimeFactory.GetCrispRuntime(resolvedPath);
                var rawResult = runtime.Run(_httpRequestConverter.ConvertHttpRequest(request));
                response = _httpRequestConverter.ConvertSymbolicExpression(request, rawResult);
            }
            catch (Exception ex)
            {
                _logger.WriteLine($"Error: {ex.Message}");

                // Return 500 internal server error.
                response = new FullHttpResponse(request.Version)
                {
                    StatusCode = 500,
                    Headers = new Dictionary<string, string>
                    {
                        {"Content-Type", "text/html"}
                    },
                    Content = _errorPageContentRetriever.GetEncodedErrorPageContent(500)
                };
            }

            return response;
        }
    }
}
