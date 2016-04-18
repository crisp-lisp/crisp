using Crisp.Shared;
using Crisp.Types;

namespace Crisp.Runtime
{
    public class CrispRuntime : ICrispRuntime
    {
        private readonly IExpressionTreeSource _expressionTreeSource;
        private readonly IEvaluator _evaluator;

        public CrispRuntime(
            IExpressionTreeSource expressionTreeSource,
            IEvaluatorFactory evaluatorFactory)
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
