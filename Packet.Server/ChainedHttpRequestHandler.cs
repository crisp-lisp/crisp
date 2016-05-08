using System.Collections.Generic;
using System.Linq;

using Packet.Interfaces.Server;

namespace Packet.Server
{
    public class ChainedHttpRequestHandler : HttpRequestHandler
    {
        public ChainedHttpRequestHandler(IEnumerable<IHttpRequestHandler> httpRequestHandlers)
        {
            // Build chain of responsibility.
            foreach (var httpRequesthandler in httpRequestHandlers.Reverse())
            {
                httpRequesthandler.Successor = Successor;
                Successor = httpRequesthandler;
            }
        }

        protected override IHttpResponse AttemptHandle(IHttpRequest request)
        {
            return null; // Defer straight to chain.
        }
    }
}
