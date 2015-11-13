using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crisp.Core.Evaluation;

namespace Crisp.Core
{
    public class Closure : Lambda
    {
        private readonly IEvaluator _evaluator;

        public override SymbolicExpression Apply(SymbolicExpression expression, IEvaluator evaluator)
        {
            return base.Apply(expression, _evaluator); // Ignore passed-in evaluator.
        }

        public Closure(IList<SymbolAtom> parameters, SymbolicExpression body, IEvaluator evaluator) : base(parameters, body)
        {
            _evaluator = evaluator;
        }
    }
}
