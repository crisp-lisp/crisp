using Crisp.Interfaces;
using Crisp.Shared;

namespace Crisp.Types
{
    /// <summary>
    /// Represents a pair-type expression that contains a head and a tail.
    /// </summary>
    public sealed class Pair : SymbolicExpression
    {
        public override bool IsAtomic => false;

        public override SymbolicExpressionType Type => SymbolicExpressionType.Pair;

        /// <summary>
        /// Gets the expression at the head of the pair.
        /// </summary>
        public ISymbolicExpression Head { get; private set; }

        /// <summary>
        /// Gets the expression at the tail of the pair.
        /// </summary>
        public ISymbolicExpression Tail { get; private set; }

        /// <summary>
        /// Whether or not the pair is explicitly bracketed.
        /// </summary>
        public bool IsExplicitlyBracketed { get; private set; }

        /// <summary>
        /// Initializes a new instance of a pair-type expression that contains a head and a tail.
        /// </summary>
        /// <param name="head">The expression to place at the head of the pair.</param>
        /// <param name="tail">The expression to place at the tail of the pair.</param>
        /// <param name="isExplicitlyBracketed">Whether or not the pair is explicitly bracketed.</param>
        public Pair(ISymbolicExpression head, ISymbolicExpression tail, bool isExplicitlyBracketed = false)
        {
            Head = head;
            Tail = tail;
            IsExplicitlyBracketed = isExplicitlyBracketed;
        }
    }
}
