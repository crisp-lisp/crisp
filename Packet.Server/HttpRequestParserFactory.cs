using Packet.Interfaces.Server;

namespace Packet.Server
{
    public static class HttpRequestParserFactory
    {
        public static IHttpRequestParser CreateHttpRequestParser()
        {
            return new HttpOnePointZeroRequestParser(new HttpZeroPointNineRequestParser(null));
        }
    }
}
