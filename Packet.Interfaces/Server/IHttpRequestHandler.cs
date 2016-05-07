using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packet.Interfaces.Server
{
    public interface IHttpRequestHandler
    {
        IHttpResponse Handle(IHttpRequest request);
    }
}
