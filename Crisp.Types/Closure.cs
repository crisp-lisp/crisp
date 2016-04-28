using System.Collections.Generic;
using Crisp.Interfaces;
using Crisp.Interfaces.Evaluation;
using Crisp.Interfaces.Types;
using Crisp.Shared;

namespace Crisp.Types
{
    /// <summary>
    /// Represents a lambda expression that will always be applied using a specific evaluator.
    /// </summary>
    public class Closure : Lambda
    {
        /// <summary>
        /// The evaluator to use to apply the lambda.
        /// </summary>
        private readonly IEvaluator _evaluator;
        
        public override ISymbolicExpression Apply(ISymbolicExpression expression, IEvaluator evaluator)
        {
            return base.Apply(expression, _evaluator); // Ignore passed-in evaluator.
        }

        /// <summary>
        /// Initializes a new instance of a lambda expression that will always be applied in a specific context.
        /// </summary>
        /// <param name="parameters">The list of parameters the lambda will take.</param>
        /// <param name="body">The body of the lambda.</param>
        /// <param name="evaluator">The evaluator to use to apply the lambda.</param>
        public Closure(IList<SymbolAtom> parameters, ISymbolicExpression body, IEvaluator evaluator)
            : base(parameters, body)
        {
            _evaluator = evaluator;
        }
    }
}
