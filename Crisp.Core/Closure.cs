using System.Collections.Generic;

using Crisp.Core.Evaluation;

namespace Crisp.Core
{
    public class Closure : Lambda
    {
        private readonly IEvaluator _evaluator;

        public override SymbolicExpression Apply(SymbolicExpression expression, IEvaluator evaluator)
        {
            return base.Apply(expression, _evaluator);
        }

        public Closure(IList<SymbolAtom> parameters, SymbolicExpression body, IEvaluator evaluator) : base(parameters, body)
        {
            _evaluator = evaluator;
        }
    }
}
