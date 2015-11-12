using System;

namespace Crisp.Core
{
    /// <summary>
    /// Represents a symbolic expression in an expression tree.
    /// </summary>
    public abstract class SymbolicExpression
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
