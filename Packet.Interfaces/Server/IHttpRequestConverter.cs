using Crisp.Interfaces.Parsing;
using Crisp.Interfaces.Types;

namespace Packet.Interfaces.Server
{
    public interface IHttpRequestConverter
    {
        IExpressionTreeSource ConvertHttpRequest(IHttpRequest request);

        IHttpResponse ConvertSymbolicExpression(IHttpRequest request, ISymbolicExpression expression);
    }
}
