using Crisp.Interfaces;
using Crisp.Interfaces.Parsing;

namespace Crisp.Shared
{
    public interface ICrispRuntime
    {
        ISymbolicExpression Run(IExpressionTreeSource argumentSource);
    }
}