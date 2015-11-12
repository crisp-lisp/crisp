using Crisp.Core.Evaluation;

namespace Crisp.Core
{
    /// <summary>
    /// Represents a binding between a symbol and an expression.
    /// </summary>
    public class Binding
    {
        /// <summary>
        /// The evaluated expression once it has been lazily evaluated.
        /// </summary>
        private SymbolicExpression _evaluated;

        /// <summary>
        /// The symbol that is bound to the expression.
        /// </summary>
        public SymbolAtom Symbol { get; private set; }

        /// <summary>
        /// The expression that is bound to the symbol.
        /// </summary>
        public SymbolicExpression Expression { get; private set; }

        /// <summary>
        /// The evaluator that should be used to evaluate the bound expression.
        /// </summary>
        public IEvaluator Evaluator { get; private set; }

        /// <summary>
        /// Evaluates the bound expression in its original context.
        /// </summary>
        /// <returns></returns>
        public SymbolicExpression Evaluate()
        {
            return _evaluated ?? (_evaluated = Evaluator.Evaluate(Expression));
        }

        /// <summary>
        /// Initializes a new instance of a binding between a symbol and an expression.
        /// </summary>
        /// <param name="symbol">The symbol that is bound to the expression.</param>
        /// <param name="expression">The expression that is bound to the symbol.</param>
        /// <param name="evaluator">The evaluator that should be used to evaluate the bound expression.</param>
        public Binding(SymbolAtom symbol, SymbolicExpression expression, IEvaluator evaluator)
        {
            Symbol = symbol;
            Expression = expression;
            Evaluator = evaluator;
        }
    }
}
