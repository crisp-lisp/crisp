using System.Collections.Generic;

using Crisp.Core.Evaluation;

namespace Crisp.Core.Types
{
    /// <summary>
    /// Represents a lambda expression.
    /// </summary>
    public class Lambda : Function
    {
        /// <summary>
        /// Contains the list of parameters that the lambda takes.
        /// </summary>
        private readonly IList<SymbolAtom> _parameters;

        /// <summary>
        /// Contains the body of the lambda.
        /// </summary>
        private readonly SymbolicExpression _body;

        public override bool SkipArgumentEvaluation => false; // Calls to lambda expressions must have arguments pre-evaluated.

        public override SymbolicExpression Apply(SymbolicExpression expression, IEvaluator evaluator)
        {
            // Make sure we've got the right number of arguments.
            var arguments = expression.AsPair().Expand();
            if (arguments.Count != _parameters.Count
                && !(arguments.Count == 1 && arguments[0].Equals(new Nil())))
            {
                throw new RuntimeException("Attempted to call lambda with wrong number of arguments.");
            }

            // Bind arguments to parameters in context.
            for (var i = 0; i < _parameters.Count; i++)
            {
                evaluator = evaluator.Bind(_parameters[i], arguments[i]);
            }

            return evaluator.Evaluate(_body);
        }

        /// <summary>
        /// Initializes a new instance of a lambda expression.
        /// </summary>
        /// <param name="parameters">The list of parameters the lambda will take.</param>
        /// <param name="body">The body of the lambda.</param>
        public Lambda(IList<SymbolAtom> parameters, SymbolicExpression body)
        {
            _parameters = parameters;
            _body = body;
        }
    }
}
