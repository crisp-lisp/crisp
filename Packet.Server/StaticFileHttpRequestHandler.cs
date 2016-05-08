using Packet.Interfaces.Logging;
using Packet.Interfaces.Server;

namespace Packet.Server
{
    public class StaticFileHttpRequestHandler : HttpRequestHandler
    {
        private readonly IUrlResolver _urlResolver;
        private readonly ILogger _logger;

        public StaticFileHttpRequestHandler(IHttpRequestHandler successor, IUrlResolver urlResolver, ILogger logger) 
            : base(successor)
        {
            _urlResolver = urlResolver;
            _logger = logger;
        }

        protected override IHttpResponse AttemptHandle(IHttpRequest request)
        {
            var resolvedPath = _urlResolver.GetUrlPath(request.Url);
            _logger.WriteLine($"Resolved URL '{request.Url}' to {resolvedPath}...");
            return new StaticFileHttpResponse(request.Version, resolvedPath);
        }
    }
}
