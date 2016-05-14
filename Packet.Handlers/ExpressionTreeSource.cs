using Crisp.Interfaces.Parsing;
using Crisp.Interfaces.Types;
using Crisp.Shared;

namespace Packet.Handlers
{
    public class ExpressionTreeSource : Provider<ISymbolicExpression>, IExpressionTreeSource
    {
        public ExpressionTreeSource(ISymbolicExpression expression) : base(expression)
        {
        }
    }
}
