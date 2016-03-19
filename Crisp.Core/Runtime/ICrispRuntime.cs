using Crisp.Core.Types;

namespace Crisp.Core.Runtime
{
    public interface ICrispRuntime
    {
        SymbolicExpression Run(string arguments);

        SymbolicExpression Run(SymbolicExpression arguments);
    }
}