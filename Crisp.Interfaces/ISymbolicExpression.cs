using Crisp.Enums;

namespace Crisp.Interfaces
{
    public interface ISymbolicExpression
    {
        /// <summary>
        /// Gets whether or not the expression is atomic.
        /// </summary>
        bool IsAtomic { get; }

        /// <summary>
        /// Gets the data type of the expression.
        /// </summary>
        SymbolicExpressionType Type { get; }
    }
}