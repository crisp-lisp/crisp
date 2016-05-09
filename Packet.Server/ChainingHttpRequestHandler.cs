using System.Collections.Generic;
using System.Linq;

using Packet.Interfaces.Server;

namespace Packet.Server
{
    /// <summary>
    /// Represents an chaining request handler.
    /// </summary>
    public class ChainingHttpRequestHandler : HttpRequestHandler
    {
        /// <summary>
        /// Initializes a new instance of a chaining request handler.
        /// </summary>
        /// <param name="httpRequestHandlers">The handlers that should form the chain of responsibility.</param>
        public ChainingHttpRequestHandler(IEnumerable<IHttpRequestHandler> httpRequestHandlers)
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
            return null; // Delegate straight to chain.
        }
    }
}
