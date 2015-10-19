namespace Crisp.Core
{
    /// <summary>
    /// Represents a node-type expression that contains a head and a tail.
    /// </summary>
    public class Node : SymbolicExpression
    {
        public override bool IsAtomic
        {
            get
            {
                return false;
            }
        }

        public override SymbolicExpressionType Type
        {
            get
            {
                return SymbolicExpressionType.Node;
            }
        }

        /// <summary>
        /// Gets the expression at the head of the node.
        /// </summary>
        public SymbolicExpression Head { get; private set; }

        /// <summary>
        /// Gets the expression at the tail of the node.
        /// </summary>
        public SymbolicExpression Tail { get; private set; }

        /// <summary>
        /// Initializes a new instance of a node-type expression that contains a head and a tail.
        /// </summary>
        /// <param name="head">The expression to place at the head of the node.</param>
        /// <param name="tail">The expression to place at the tail of the node.</param>
        public Node(SymbolicExpression head, SymbolicExpression tail)
        {
            Head = head;
            Tail = tail;
        }
    }
}
