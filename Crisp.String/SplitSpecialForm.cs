using System.Collections.Generic;
using System.Linq;

using Crisp.Shared;
using Crisp.Types;

namespace Crisp.String
{
    /// <summary>
    /// Returns a list of substrings created by splitting a string along a delimiter.
    /// </summary>
    public class SplitSpecialForm : SpecialForm
    {
        public override IEnumerable<string> Names => new List<string> { "split" };

        public override ISymbolicExpression Apply(ISymbolicExpression expression, IEvaluator evaluator)
        {
            expression.ThrowIfNotList(Name); // Takes a list of arguments.

            var arguments = expression.AsPair().Expand();
            arguments.ThrowIfWrongLength(Name, 2); // Must have two arguments.

            // Attempt to evaluate every argument to a string.
            var evaluated = arguments.Select(evaluator.Evaluate).ToArray();
            if (evaluated.Any(e => e.Type != SymbolicExpressionType.String))
            {
                throw new FunctionApplicationException(
                    $"The arguments to the function '{Name}' must all evaluate to the string type.");
            }

            var split = evaluated[0].AsString().Value.Split(evaluated[1].AsString().Value.ToCharArray());

            var atoms = new List<ISymbolicExpression>();
            atoms.AddRange(split.Select(s => new StringAtom(s)));

            return atoms.ToProperList();
        }
    }
}
