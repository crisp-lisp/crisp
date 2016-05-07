namespace Packet.Interfaces.Server
{
    public interface IHttpRequestHandler
    {
        IHttpResponse Handle(IHttpRequest request);
    }
}
