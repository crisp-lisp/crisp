using Packet.Interfaces.Server;

namespace Packet.Handlers
{
    public abstract class HttpRequestHandler : IHttpRequestHandler
    {
        public IHttpRequestHandler Successor { get; set; }
        
        /// <summary>
        /// Attempts to compute and return and response to the given request, returns null in case of failure.
        /// </summary>
        /// <param name="request">The request to attempt to compute a response to.</param>
        /// <returns></returns>
        protected abstract IHttpResponse AttemptHandle(IHttpRequest request);

        public IHttpResponse Handle(IHttpRequest request)
        {
            // Chain of responsibility.
            return AttemptHandle(request) ?? Successor?.Handle(request);
        }
    }
}
