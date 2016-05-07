using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Packet.Interfaces.Server;

namespace Packet.Server
{
    public abstract class HttpRequestHandler : IHttpRequestHandler
    {
        public IHttpRequestHandler Successor { get; }

        protected HttpRequestHandler(IHttpRequestHandler successor)
        {
            Successor = successor;
        }

        protected abstract IHttpResponse AttemptHandle(IHttpRequest request);

        public IHttpResponse Handle(IHttpRequest request)
        {
            return AttemptHandle(request) ?? Successor?.Handle(request);
        }
    }
}
