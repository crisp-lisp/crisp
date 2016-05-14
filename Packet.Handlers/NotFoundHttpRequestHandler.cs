using System.Collections.Generic;
using System.IO;
using System.Text;

using Packet.Interfaces.Configuration;
using Packet.Interfaces.Server;

namespace Packet.Handlers
{
    /// <summary>
    /// Represents a HTTP handler that serves 404 not found responses for nonexistent resources.
    /// </summary>
    public class NotFoundHttpRequestHandler : HttpRequestHandler
    {
        private readonly IPacketConfiguration _packetConfiguration;

        private readonly IUrlResolver _urlResolver;

        /// <summary>
        /// Initializes a new instance of a HTTP handler that serves 404 not found responses for nonexistent resources.
        /// </summary>
        /// <param name="packetConfigurationProvider">The server configuration provider service.</param>
        /// <param name="urlResolver">The URL resolver service.</param>
        public NotFoundHttpRequestHandler(
            IPacketConfigurationProvider packetConfigurationProvider,
            IUrlResolver urlResolver)
        {
            _packetConfiguration = packetConfigurationProvider.Get();
            _urlResolver = urlResolver;
        }

        protected override IHttpResponse AttemptHandle(IHttpRequest request)
        {
            var resolvedPath = _urlResolver.Resolve(request.Url); // Resolve URL.

            // Defer if file exists.
            if (File.Exists(resolvedPath))
            {
                return null;
            }

            // Resolve URL of custom error page.
            var notFoundPagePath = _urlResolver.Resolve(_packetConfiguration.NotFoundErrorPage);

            // If custom error page not found, use default.
            var content = File.Exists(notFoundPagePath)
                ? File.ReadAllBytes(notFoundPagePath)
                : new UTF8Encoding().GetBytes(Properties.Resources.DefaultErrorPage_404);

            return new FullHttpResponse(request.Version)
            {
                StatusCode = 404,
                Headers = new Dictionary<string, string>
                {
                    {"Content-Type", "text/html"}
                },
                Content = content
            };
        }
    }
}
