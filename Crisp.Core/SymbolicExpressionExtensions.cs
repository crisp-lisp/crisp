namespace Crisp.Core
{
    /// <summary>
    /// Contains convenient extension methods for working with symbolic expressions.
    /// </summary>
    public static class SymbolicExpressionExtensions
    {
        /// <summary>
        /// Gets the expression as a pair or throws an exception in case of type mismatch.
        /// </summary>
        /// <param name="expression">The expression to attempt to cast.</param>
        /// <returns></returns>
        public static Pair AsPair(this SymbolicExpression expression)
        {
            return (Pair)expression;
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
        /// Gets the expression as a constant atom or throws an exception in case of type mismatch.
        /// </summary>
        /// <param name="expression">The expression to attempt to cast.</param>
        /// <returns></returns>
        public static ConstantAtom AsConstant(this SymbolicExpression expression)
        {
            return (ConstantAtom)expression;
        }

        /// <summary>
        /// Gets the expression as a function or throws an exception in the case of type mismatch.
        /// </summary>
        /// <param name="expression">The expression to attempt to case.</param>
        /// <returns></returns>
        public static IFunction AsFunction(this SymbolicExpression expression)
        {
            return (IFunction)expression;
        }

        /// <summary>
        /// Returns the head of a pair as a pair.
        /// </summary>
        /// <param name="expression">The pair to navigate.</param>
        /// <returns></returns>
        public static Pair GoHead(this Pair expression)
        {
            return expression.AsPair().Head.AsPair();
        }

        /// <summary>
        /// Returns the tail of a pair as a pair.
        /// </summary>
        /// <param name="expression">The pair to navigate.</param>
        /// <returns></returns>
        public static Pair GoTail(this Pair expression)
        {
            return expression.AsPair().Tail.AsPair();
        }
    }
}
