using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Crisp.Enums;
using Crisp.Interfaces.Types;
using Crisp.IoC;
using Crisp.Types;

using Packet.Enums;
using Packet.Interfaces.Configuration;
using Packet.Interfaces.Logging;
using Packet.Interfaces.Server;

namespace Packet.Server
{
    /// <summary>
    /// Represents a HTTP request handler for serving dynamic pages.
    /// </summary>
    public class DynamicPageHttpRequestHandler : HttpRequestHandler
    {
        private readonly IPacketConfiguration _packetConfiguration;

        private readonly IUrlResolver _urlResolver;

        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of a HTTP request handler for serving dynamic pages.
        /// </summary>
        /// <param name="packetConfigurationProvider">The server configuration provider service.</param>
        /// <param name="urlResolver">The URL resolution service.</param>
        /// <param name="logger"></param>
        public DynamicPageHttpRequestHandler(
            IPacketConfigurationProvider packetConfigurationProvider,
            IUrlResolver urlResolver,
            ILogger logger)
        {
            _packetConfiguration = packetConfigurationProvider.Get(); 
            _urlResolver = urlResolver;
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

        /// <summary>
        /// Converts a name-value collection of HTTP headers passed back by a Crisp webpage to a dictionary.
        /// </summary>
        /// <param name="headers">The symbolic expression containingthe name-value collection to convert.</param>
        /// <returns></returns>
        private static Dictionary<string, string> TransformHeadersForPacket(ISymbolicExpression headers)
        {
            if (headers.Type == SymbolicExpressionType.Nil)
            {
                return new Dictionary<string, string>();
            }

            var expanded = headers.AsPair().Expand();
            return expanded.ToDictionary(p => p.AsPair().Head.AsString().Value, p => p.AsPair().Tail.AsString().Value);
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
            IList<ISymbolicExpression> result;
            try
            {
                // Create runtime for file.
                var runtime = CrispRuntimeFactory.GetCrispRuntime(resolvedPath);
                result = runtime.Run(new HttpExpressionTreeSource(request)).AsPair().Expand();
            }
            catch (Exception ex)
            {
                // Log error.
                _logger.WriteLine($"Error: {ex.Message}");

                // Resolve URL of custom error page.
                var internalServerErrorPagePath = _urlResolver.Resolve(_packetConfiguration.InternalServerErrorPage);

                // If custom error page not found, use default.
                var errorPageContent = File.Exists(internalServerErrorPagePath)
                    ? File.ReadAllBytes(internalServerErrorPagePath)
                    : new UTF8Encoding().GetBytes(Properties.Resources.DefaultErrorPage_500);

                return new FullHttpResponse(request.Version)
                {
                    StatusCode = 500,
                    Headers = new Dictionary<string, string>
                    {
                        {"Content-Type", "text/html"}
                    },
                    Content = errorPageContent
                };
            }

            // Automatic headers.
            var autoHeaders = new Dictionary<string, string>
            {
                {"Content-Type", result[2].AsString().Value}
            };
            var allHeaders = autoHeaders.Concat(TransformHeadersForPacket(result[3])
                .Where(h => !autoHeaders.ContainsKey(h.Key)))
                .ToDictionary(h => h.Key, h => h.Value);

            // Encode in UTF-8.
            // TODO: Encoding.
            var content = new UTF8Encoding().GetBytes(result[0].AsString().Value);

            // Simple request means simple response.
            if (request.Version.Major < 1)
            {
                return new SimpleHttpResponse(content);
            }

            // Full request means full response.
            return new FullHttpResponse(request.Version)
            {
                StatusCode = Convert.ToInt32(result[1].AsNumeric().Value),
                Content = content,
                Headers = allHeaders
            };
        }
    }
}
