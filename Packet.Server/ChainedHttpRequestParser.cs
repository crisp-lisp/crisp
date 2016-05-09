using System.Collections.Generic;
using System.Linq;

using Packet.Interfaces.Server;

namespace Packet.Server
{
    public class ChainedHttpRequestParser : HttpRequestParser
    {
        public ChainedHttpRequestParser(IEnumerable<IHttpRequestParser> httpRequestParsers)
        {
            // Build chain of responsibility.
            foreach (var httpRequestParser in httpRequestParsers.Reverse())
            {
                httpRequestParser.Successor = Successor;
                Successor = httpRequestParser;
            }
        }

        protected override IHttpRequest AttemptParse(byte[] request)
        {
            return null; // Delegate straight to chain.
        }

        protected override IHttpVersion AttemptGetVersion(string requestLine)
        {
            return null; // Delegate straight to chain.
        }
    }
}
