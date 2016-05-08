using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Packet.Interfaces.Server;

namespace Packet.Server
{
    public class StaticFileHttpRequestHandler : HttpRequestHandler
    {
        public StaticFileHttpRequestHandler(IHttpRequestHandler successor) : base(successor)
        {
        }

        protected override IHttpResponse AttemptHandle(IHttpRequest request)
        {
            
        }
    }
}
