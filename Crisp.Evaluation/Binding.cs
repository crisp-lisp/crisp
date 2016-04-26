using Crisp.Interfaces;
using Crisp.Interfaces.Evaluation;

namespace Crisp.Evaluation
{
    public class Binding : IBinding
    {
        private ISymbolicExpression _evaluated;

        public string Name { get; }

        public ISymbolicExpression Expression { get; }

        public IEvaluator Evaluator { get; }
        
        public ISymbolicExpression Value => _evaluated ?? (_evaluated = Evaluator.Evaluate(Expression));

        /// <summary>
        /// Initializes a new instance of a binding between a name and an expression.
        /// </summary>
        /// <param name="name">The name that is bound to the expression.</param>
        /// <param name="expression">The expression that is bound to the name.</param>
        /// <param name="evaluator">The evaluator to use when the bound expression is lazily evaluated.</param>
        public Binding(string name, ISymbolicExpression expression, IEvaluator evaluator)
        {
            Name = name;
            Expression = expression;
            Evaluator = evaluator;
        }
    }
}
