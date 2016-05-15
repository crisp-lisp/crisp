using System.Collections.Generic;
using System.Linq;

using Crisp.Interfaces.Evaluation;
using Crisp.Interfaces.Types;
using Crisp.Types;

namespace Crisp.Basic
{
    /// <summary>
    /// A special form that sequentially evaluates its arguments, returning the result of evaluation of its last
    /// argument.
    /// </summary>
    /// <remarks>Useful for programming with side-effects.</remarks>
    public class ProgSpecialForm : SpecialForm
    {
        public override IEnumerable<string> Names => new List<string> {"prog"};

        public override ISymbolicExpression Apply(ISymbolicExpression expression, IEvaluator evaluator)
        {
            expression.ThrowIfNotList(Name); // Takes a list of arguments.

            var evaluable = expression.AsPair().Expand();
            var evaluated = evaluable.Select(evaluator.Evaluate); // Evaluate all arguments once.

            return evaluated.Last(); // Return last item.
        }
    }
}
