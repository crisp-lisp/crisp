using System.Collections.Generic;
using System.Linq;

using Packet.Interfaces.Server;

namespace Packet.Server
{
    /// <summary>
    /// Represents a chaining HTTP request parser.
    /// </summary>
    public class ChainedHttpRequestParser : HttpRequestParser
    {
        /// <summary>
        /// Initializes a new instance of a chaining HTTP request parser.
        /// </summary>
        /// <param name="httpRequestParsers">The parsers that should form the chain of responsibility.</param>
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
