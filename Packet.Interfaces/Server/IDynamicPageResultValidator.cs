using Crisp.Interfaces.Types;

namespace Packet.Interfaces.Server
{
    public interface IDynamicPageResultValidator
    {
        bool Validate(ISymbolicExpression result);
    }
}
