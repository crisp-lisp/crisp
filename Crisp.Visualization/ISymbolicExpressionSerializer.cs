using Crisp.Shared;

namespace Crisp.Visualization
{
    public interface ISymbolicExpressionSerializer
    {
        string Serialize(ISymbolicExpression expression);
    }
}
