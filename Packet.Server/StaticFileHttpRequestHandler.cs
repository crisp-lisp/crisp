using Packet.Interfaces.Configuration;
using Packet.Interfaces.Logging;
using Packet.Interfaces.Server;

namespace Packet.Server
{
    public class StaticFileHttpRequestHandler : HttpRequestHandler
    {
        private readonly IPacketConfiguration _packetConfiguration;
        private readonly IUrlResolver _urlResolver;
        private readonly ILogger _logger;

        public StaticFileHttpRequestHandler(
            IPacketConfigurationProvider packetConfigurationProvider, 
            IUrlResolver urlResolver, 
            ILogger logger) 
        {
            _packetConfiguration = packetConfigurationProvider.Get();
            _urlResolver = urlResolver;
            _logger = logger;
        }

        protected override IHttpResponse AttemptHandle(IHttpRequest request)
        {
            var resolvedPath = _urlResolver.Resolve(request.Url);
            _logger.WriteLine($"Resolved URL '{request.Url}' to {resolvedPath}...");
            return new StaticFileHttpResponse(request.Version, resolvedPath);
        }
    }
}
