using Crisp.Interfaces;
using Crisp.Shared;

namespace Crisp.Types
{
    /// <summary>
    /// Represents a nil expression.
    /// </summary>
    public sealed class Nil : SymbolicExpression
    {
        public override bool IsAtomic => true;

        public override SymbolicExpressionType Type => SymbolicExpressionType.Nil;

        public override bool Equals(object obj)
        {
            var atom = obj as Nil;
            return atom != null;
        }

        public override int GetHashCode()
        {
            return 0;
        }
    }
}
