namespace Crisp.Core
{
    /// <summary>
    /// Represents a pair-type expression that contains a head and a tail.
    /// </summary>
    public class Pair : SymbolicExpression
    {
        public override bool IsAtomic => false;

        public override SymbolicExpressionType Type => SymbolicExpressionType.Pair;

        /// <summary>
        /// Gets the expression at the head of the pair.
        /// </summary>
        public SymbolicExpression Head { get; private set; }

        /// <summary>
        /// Gets the expression at the tail of the pair.
        /// </summary>
        public SymbolicExpression Tail { get; private set; }

        /// <summary>
        /// Initializes a new instance of a pair-type expression that contains a head and a tail.
        /// </summary>
        /// <param name="head">The expression to place at the head of the pair.</param>
        /// <param name="tail">The expression to place at the tail of the pair.</param>
        public Pair(SymbolicExpression head, SymbolicExpression tail)
        {
            Head = head;
            Tail = tail;
        }
    }
}
