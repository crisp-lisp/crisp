using Crisp.Interfaces;
using Crisp.Interfaces.Parsing;
using Crisp.Interfaces.Types;

namespace Crisp.Shared
{
    public interface ICrispRuntime
    {
        ISymbolicExpression Run(IExpressionTreeSource argumentSource);
    }
}