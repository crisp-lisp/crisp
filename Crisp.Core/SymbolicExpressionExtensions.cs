namespace Crisp.Core
{
    /// <summary>
    /// Contains convenient extension methods for working with symbolic expressions.
    /// </summary>
    public static class SymbolicExpressionExtensions
    {
        /// <summary>
        /// Gets the expression as a node or throws an exception in case of type mismatch.
        /// </summary>
        /// <param name="expression">The expression to attempt to cast.</param>
        /// <returns></returns>
        public static Node AsNode(this SymbolicExpression expression)
        {
            return (Node)expression;
        }

        /// <summary>
        /// Gets the expression as a string atom or throws an exception in case of type mismatch.
        /// </summary>
        /// <param name="expression">The expression to attempt to cast.</param>
        /// <returns></returns>
        public static StringAtom AsString(this SymbolicExpression expression)
        {
            return (StringAtom)expression;
        }

        /// <summary>
        /// Gets the expression as a numeric atom or throws an exception in case of type mismatch.
        /// </summary>
        /// <param name="expression">The expression to attempt to cast.</param>
        /// <returns></returns>
        public static NumericAtom AsNumeric(this SymbolicExpression expression)
        {
            return (NumericAtom)expression;
        }

        /// <summary>
        /// Gets the expression as a symbolic atom or throws an exception in case of type mismatch.
        /// </summary>
        /// <param name="expression">The expression to attempt to cast.</param>
        /// <returns></returns>
        public static SymbolAtom AsSymbol(this SymbolicExpression expression)
        {
            return (SymbolAtom)expression;
        }

        /// <summary>
        /// Returns the head of a node as a node.
        /// </summary>
        /// <param name="expression">The node to navigate.</param>
        /// <returns></returns>
        public static Node GoHead(this Node expression)
        {
            return expression.AsNode().Head.AsNode();
        }

        /// <summary>
        /// Returns the tail of a node as a node.
        /// </summary>
        /// <param name="expression">The node to navigate.</param>
        /// <returns></returns>
        public static Node GoTail(this Node expression)
        {
            return expression.AsNode().Tail.AsNode();
        }
    }
}
