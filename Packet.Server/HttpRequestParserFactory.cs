using Packet.Interfaces.Server;

namespace Packet.Server
{
    public static class HttpRequestParserFactory
    {
        public static IHttpRequestParser CreateHttpRequestParser()
        {
            // Chain of responsibility.
            return new FullHttpRequestParser(
                new SimpleRequestParser(null));
        }
    }
}
