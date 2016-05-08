namespace Packet.Interfaces.Server
{
    public interface IHttpRequestHandler
    {
        IHttpRequestHandler Successor { get; set; }

        IHttpResponse Handle(IHttpRequest request);
    }
}
