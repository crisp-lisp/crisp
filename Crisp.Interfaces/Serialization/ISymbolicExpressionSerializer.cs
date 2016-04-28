using Crisp.Interfaces.Types;

namespace Crisp.Interfaces.Serialization
{
    public interface ISymbolicExpressionSerializer
    {
        string Serialize(ISymbolicExpression expression);
    }
}
