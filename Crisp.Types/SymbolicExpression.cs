using Crisp.Enums;
using Crisp.Interfaces;
using Crisp.Shared;

namespace Crisp.Types
{
    /// <summary>
    /// Represents a symbolic expression in an expression tree.
    /// </summary>
    public abstract class SymbolicExpression : ISymbolicExpression
    {
        /// <summary>
        /// Gets whether or not the expression is atomic.
        /// </summary>
        public abstract bool IsAtomic { get; }
        
        /// <summary>
        /// Gets the data type of the expression.
        /// </summary>
        public abstract SymbolicExpressionType Type { get; }
    }
}
