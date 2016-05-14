using System.Collections.Generic;
using System.IO;

using Packet.Interfaces.Configuration;
using Packet.Interfaces.Server;

namespace Packet.Handlers
{
    /// <summary>
    /// Represents a HTTP handler that serves static files.
    /// </summary>
    public class StaticFileHttpRequestHandler : HttpRequestHandler
    {
        private readonly IPacketConfiguration _packetConfiguration;

        private readonly IUrlResolver _urlResolver;

        /// <summary>
        /// Initializes a new instance of a HTTP handler that serves static files.
        /// </summary>
        /// <param name="packetConfigurationProvider">The server configuration provider service.</param>
        /// <param name="urlResolver">The URL resolver service.</param>
        public StaticFileHttpRequestHandler(
            IPacketConfigurationProvider packetConfigurationProvider, 
            IUrlResolver urlResolver) 
        {
            _packetConfiguration = packetConfigurationProvider.Get();
            _urlResolver = urlResolver;
        }

        /// <summary>
        /// Gets the MIME type for the given file extension.
        /// </summary>
        /// <param name="extension">The file extension to return the MIME type for.</param>
        /// <returns></returns>
        private string GetMimeTypeForExtension(string extension)
        {
            string value;
            return _packetConfiguration.MimeTypeMappings.TryGetValue(extension, out value) ?
                value : "application/octet-stream"; // Default to this MIME type.
        }

        protected override IHttpResponse AttemptHandle(IHttpRequest request)
        {
            var resolvedPath = _urlResolver.Resolve(request.Url); // Resolve URL.

            return new FullHttpResponse(request.Version)
            {
                StatusCode = 200,
                Headers = new Dictionary<string, string>
                {
                    {"Content-Type", GetMimeTypeForExtension(Path.GetExtension(resolvedPath))} // Use correct MIME.
                },
                Content = File.ReadAllBytes(resolvedPath)
            };
        }
    }
}
