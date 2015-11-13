using Crisp.Core.Evaluation;

namespace Crisp.Core
{
    /// <summary>
    /// Represents a binding between a symbol and an expression.
    /// </summary>
    public class Binding
    {
        private readonly IEvaluator _evaluator;

        private readonly SymbolicExpression _expression;

        /// <summary>
        /// The symbol that is bound to the expression.
        /// </summary>
        public SymbolAtom Symbol { get; private set; }

        /// <summary>
        /// The expression that is bound to the symbol.
        /// </summary>
        public SymbolicExpression Value => _evaluator.Evaluate(_expression);
        
        /// <summary>
        /// Initializes a new instance of a binding between a symbol and an expression.
        /// </summary>
        /// <param name="symbol">The symbol that is bound to the expression.</param>
        /// <param name="expression">The expression that is bound to the symbol.</param>
        /// <param name="evaluator"></param>
        public Binding(SymbolAtom symbol, SymbolicExpression expression, IEvaluator evaluator)
        {
            Symbol = symbol;
            _expression = expression;
            _evaluator = evaluator;
        }
    }
}
