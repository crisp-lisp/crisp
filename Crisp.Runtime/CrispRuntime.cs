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
            var func = _evaluator.Evaluate(_expressionTreeSource.Get()).AsFunction();
            return func.Apply(argumentSource.Get(), null);
        }
    }
}
