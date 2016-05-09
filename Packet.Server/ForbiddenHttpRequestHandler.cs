using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using Packet.Interfaces.Configuration;
using Packet.Interfaces.Server;

namespace Packet.Server
{
    /// <summary>
    /// Represents a HTTP handler that serves 403 forbidden responses for forbidden resources.
    /// </summary>
    public class ForbiddenHttpRequestHandler : HttpRequestHandler
    {
        private readonly IPacketConfiguration _packetConfiguration;

        private readonly IUrlResolver _urlResolver;
        
        /// <summary>
        /// Initializes a new instance of a HTTP handler that serves 403 forbidden responses for forbidden resources.
        /// </summary>
        /// <param name="packetConfigurationProvider">The server configuration provider service.</param>
        /// <param name="urlResolver">The URL resolver service.</param>
        public ForbiddenHttpRequestHandler( 
            IPacketConfigurationProvider packetConfigurationProvider,
            IUrlResolver urlResolver)
        {
            _packetConfiguration = packetConfigurationProvider.Get();
            _urlResolver = urlResolver;
        }

        /// <summary>
        /// Returns true if the given path matches a 'do not serve' rule. Otherwise returns false.
        /// </summary>
        /// <param name="path">The path to check.</param>
        /// <returns></returns>
        private bool IsForbiddenPath(string path)
        {
            return _packetConfiguration.DoNotServePatterns.Any(p => Regex.IsMatch(path, p));
        }

        protected override IHttpResponse AttemptHandle(IHttpRequest request)
        {
            var resolvedPath = _urlResolver.Resolve(request.Url); // Resolve URL.

            // Defer if file is not forbidden.
            if (!IsForbiddenPath(resolvedPath))
            {
                return null;
            }
            
            // Resolve URL of custom error page.
            var forbiddenPagePath = _urlResolver.Resolve(_packetConfiguration.ForbiddenErrorPage);

            // If custom error page not found, use default.
            var content = File.Exists(forbiddenPagePath)
                ? File.ReadAllBytes(forbiddenPagePath)
                : new UTF8Encoding().GetBytes(Properties.Resources.DefaultErrorPage_403);

            return new FullHttpResponse(request.Version)
            {
                StatusCode = 403,
                Headers = new Dictionary<string, string>
                {
                    {"Content-Type", "text/html"}
                },
                Content = content
            };
        }
    }
}
