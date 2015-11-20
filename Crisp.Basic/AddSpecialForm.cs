using System.Linq;

using Crisp.Core;
using Crisp.Core.Evaluation;

namespace Crisp.Basic
{
    /// <summary>
    /// Represents the basic addition function.
    /// </summary>
    public class AddSpecialForm : SpecialForm
    {
        public override string Name => "add";

        public override SymbolicExpression Apply(SymbolicExpression expression, IEvaluator evaluator)
        {
            expression.ThrowIfNotList(Name); // Takes a list of arguments.

            var arguments = expression.AsPair().Expand();
            arguments.ThrowIfWrongLength(Name, 2); // Must have two arguments.

            // Attempt to evaluate every argument to a number.
            var evaluated = arguments.Select(evaluator.Evaluate).ToArray();
            if (evaluated.Any(e => e.Type != SymbolicExpressionType.Numeric))
            {
                throw new RuntimeException(
                    $"The arguments to the function '{Name}' must all evaluate to the numeric type.");
            }

            return new NumericAtom(evaluated[0].AsNumeric().Value + evaluated[1].AsNumeric().Value);
        }
    }
}
