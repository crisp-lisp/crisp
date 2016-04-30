using Packet.Interfaces.Server;

namespace Packet.Server
{
    public abstract class HttpRequestParser : IHttpRequestParser
    {
        public IHttpRequestParser Successor { get; }

        protected HttpRequestParser(IHttpRequestParser successor)
        {
            Successor = successor;
        }

        protected abstract IHttpRequest AttemptParse(byte[] request);

        public IHttpRequest Parse(byte[] request)
        {
            return AttemptParse(request) ?? Successor?.Parse(request);
        }
    }
}
