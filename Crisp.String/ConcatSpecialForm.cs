using System.Linq;

using Crisp.Core;
using Crisp.Core.Evaluation;
using Crisp.Core.Types;

namespace Crisp.String
{
    /// <summary>
    /// Represents the basic string concatenation function.
    /// </summary>
    public class ConcatSpecialForm : SpecialForm
    {
        public override string Name => "concat";

        public override SymbolicExpression Apply(SymbolicExpression expression, IEvaluator evaluator)
        {
            expression.ThrowIfNotList(Name); // Takes a list of arguments.

            var arguments = expression.AsPair().Expand();
            arguments.ThrowIfWrongLength(Name, 2); // Must have two arguments.

            // Attempt to evaluate every argument to a string.
            var evaluated = arguments.Select(evaluator.Evaluate).ToArray();
            if (evaluated.Any(e => e.Type != SymbolicExpressionType.String))
            {
                throw new RuntimeException(
                    $"The arguments to the function '{Name}' must all evaluate to the string type.");
            }

            var concat = evaluated[0].AsString().Value + evaluated[1].AsString().Value;
            return new StringAtom(concat);
        }
    }
}
