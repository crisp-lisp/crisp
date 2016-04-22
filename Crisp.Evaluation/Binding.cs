using Crisp.Shared;

namespace Crisp.Evaluation
{
    /// <summary>
    /// A binding between a name and an expression.
    /// </summary>
    public class Binding : IBinding
    {
        private readonly IEvaluator _evaluator;
        
        private readonly ISymbolicExpression _expression;

        /// <summary>
        /// Holds the expression once it has been lazily evaluated.
        /// </summary>
        private ISymbolicExpression _evaluated;

        /// <summary>
        /// Gets the name that is bound to the expression.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the expression that is bound to the symbol.
        /// </summary>
        public ISymbolicExpression Value => _evaluated ?? (_evaluated = _evaluator.Evaluate(_expression));

        /// <summary>
        /// Initializes a new instance of a binding between a name and an expression.
        /// </summary>
        /// <param name="name">The name that is bound to the expression.</param>
        /// <param name="expression">The expression that is bound to the name.</param>
        /// <param name="evaluator">The evaluator to use when the bound expression is lazily evaluated.</param>
        public Binding(string name, ISymbolicExpression expression, IEvaluator evaluator)
        {
            Name = name;
            _expression = expression;
            _evaluator = evaluator;
        }
    }
}
