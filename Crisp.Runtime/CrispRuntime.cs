using Crisp.Interfaces.Evaluation;
using Crisp.Interfaces.Parsing;
using Crisp.Interfaces.Runtime;
using Crisp.Interfaces.Types;
using Crisp.Types;

namespace Crisp.Runtime
{
    public class CrispRuntime : ICrispRuntime
    {
        private readonly IExpressionTreeSource _expressionTreeSource;

        private readonly IEvaluator _evaluator;

        /// <summary>
        /// Initializes a new instance of a runtime capable of running Crisp programs.
        /// </summary>
        /// <param name="expressionTreeSource">The source of the program expression tree.</param>
        /// <param name="evaluatorFactory">The factory service to use to create the evaluator.</param>
        public CrispRuntime(IExpressionTreeSource expressionTreeSource, IEvaluatorFactory evaluatorFactory)
        {
            _expressionTreeSource = expressionTreeSource;
            _evaluator = evaluatorFactory.Get();
        }

        public ISymbolicExpression Run(IExpressionTreeSource argumentSource)
        {
            // Evaluate tree to get program.
            var program = _evaluator.Evaluate(_expressionTreeSource.Get()).AsFunction();

            // Return result of applying program to arguments.
            return program.Apply(argumentSource.Get(), null);
        }
    }
}
