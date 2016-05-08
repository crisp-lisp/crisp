using Packet.Interfaces.Server;

namespace Packet.Server
{
    public abstract class HttpRequestHandler : IHttpRequestHandler
    {
        public IHttpRequestHandler Successor { get; set; }
        
        protected abstract IHttpResponse AttemptHandle(IHttpRequest request);

        public IHttpResponse Handle(IHttpRequest request)
        {
            return AttemptHandle(request) ?? Successor?.Handle(request);
        }
    }
}
