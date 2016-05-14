using Crisp.Interfaces.Parsing;

namespace Packet.Interfaces.Server
{
    public interface IHttpRequestConverter
    {
        IExpressionTreeSource ConvertHttpRequest(IHttpRequest request);
    }
}
