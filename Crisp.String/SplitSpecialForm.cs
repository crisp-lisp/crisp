using System.Collections.Generic;
using System.Linq;

using Crisp.Core;
using Crisp.Core.Evaluation;
using Crisp.Core.Types;

namespace Crisp.String
{
    /// <summary>
    /// Returns a list of substrings created by splitting a string along a delimiter.
    /// </summary>
    public class SplitSpecialForm : SpecialForm
    {
        public override string Name => "split";

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

            var split = evaluated[0].AsString().Value.Split(evaluated[1].AsString().Value.ToCharArray());

            var atoms = new List<SymbolicExpression>();
            atoms.AddRange(split.Select(s => new StringAtom(s)));

            return atoms.ToProperList();
        }
    }
}
