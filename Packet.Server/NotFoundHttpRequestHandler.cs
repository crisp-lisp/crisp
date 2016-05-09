using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using Packet.Interfaces.Configuration;
using Packet.Interfaces.Server;

namespace Packet.Server
{
    /// <summary>
    /// Represents a HTTP handler that serves 404 not found responses for nonexistent resources.
    /// </summary>
    public class NotFoundHttpRequestHandler : HttpRequestHandler
    {
        private readonly IUrlResolver _urlResolver;

        private readonly IPacketConfiguration _packetConfiguration;

        /// <summary>
        /// Initializes a new instance of a HTTP handler that serves 404 not found responses for nonexistent resources.
        /// </summary>
        /// <param name="urlResolver">The URL resolver service.</param>
        /// <param name="packetConfigurationProvider">The server configuration provider service.</param>
        public NotFoundHttpRequestHandler(
            IUrlResolver urlResolver, 
            IPacketConfigurationProvider packetConfigurationProvider)
        {
            _urlResolver = urlResolver;
            _packetConfiguration = packetConfigurationProvider.Get();
        }

        protected override IHttpResponse AttemptHandle(IHttpRequest request)
        {
            var resolvedPath = _urlResolver.Resolve(request.Url);

            // Defer if file exists.
            if (File.Exists(resolvedPath))
            {
                return null;
            }

            // TODO: Exists?
            var notFoundPagePath = _urlResolver.Resolve(_packetConfiguration.NotFoundErrorPage);

            return new FullHttpResponse(request.Version)
            {
                StatusCode = 404,
                Headers = new Dictionary<string, string>
                {
                    {"Content-Type", "text/html"}
                },
                Content = File.ReadAllBytes(notFoundPagePath) 
            };
        }
    }
}
