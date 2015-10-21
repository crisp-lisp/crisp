namespace Crisp.Core
{
    /// <summary>
    /// Represents a binding between a symbol and an expression.
    /// </summary>
    public class Binding
    {
        /// <summary>
        /// The symbol that is bound to the expression.
        /// </summary>
        public SymbolAtom Symbol { get; private set; }

        /// <summary>
        /// The expression that is bound to the symbol.
        /// </summary>
        public SymbolicExpression Expression { get; private set; }

        /// <summary>
        /// Initializes a new instance of a binding between a symbol and an expression.
        /// </summary>
        /// <param name="symbol">The symbol that is bound to the expression.</param>
        /// <param name="expression">The expression that is bound to the symbol.</param>
        public Binding(SymbolAtom symbol, SymbolicExpression expression)
        {
            Symbol = symbol;
            Expression = expression;
        }
    }
}
