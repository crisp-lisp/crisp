using System.Linq;

using Crisp.Core;
using Crisp.Core.Evaluation;
using Crisp.Core.Types;

namespace Crisp.Basic
{
    /// <summary>
    /// A function that sequentially evaluates its arguments, returning the result of evaluation of its last argument.
    /// </summary>
    /// <remarks>Useful for programming with side-effects.</remarks>
    public class ProgSpecialForm : SpecialForm
    {
        public override string Name => "prog";

        public override SymbolicExpression Apply(SymbolicExpression expression, IEvaluator evaluator)
        {
            expression.ThrowIfNotList(Name); // Takes a list of arguments.

            var evaluable = expression.AsPair().Expand();
            var evaluated = evaluable.Select(evaluator.Evaluate); // Evaluate all arguments once.

            return evaluated.Last(); // Return last item.
        }
    }
}
