using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Packet.Interfaces.Server;

namespace Packet.Server
{
    public static class HttpParserFactory
    {
        public static IHttpRequestParser CreateHttpRequestParser()
        {
            return new SimpleHttpRequestParser(null);
        }
    }
}
