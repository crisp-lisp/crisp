using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Crisp.Shared;
using Crisp.Types;

namespace Crisp.String
{
    /// <summary>
    /// Returns a string atom consisting of the first argument passed to this function with all matches to the regular
    /// expression specified as the second argument replaced with the string passed as the third argument.
    /// </summary>
    public class RegexReplaceSpecialForm : SpecialForm
    {
        public override IEnumerable<string> Names => new List<string> { "regex-replace" };

        public override ISymbolicExpression Apply(ISymbolicExpression expression, IEvaluator evaluator)
        {
            expression.ThrowIfNotList(Name); // Takes a list of arguments.

            var arguments = expression.AsPair().Expand();
            arguments.ThrowIfWrongLength(Name, 3); // Must have three arguments.

            // Attempt to evaluate every argument to a string.
            var evaluated = arguments.Select(evaluator.Evaluate).ToArray();
            if (evaluated.Any(e => e.Type != SymbolicExpressionType.String))
            {
                throw new FunctionApplicationException(
                    $"The arguments to the function '{Name}' must all evaluate to the string type.");
            }

            var subject = evaluated[0].AsString().Value;
            var search = evaluated[1].AsString().Value;
            var replacement = evaluated[2].AsString().Value;

            return new StringAtom(Regex.Replace(subject, search, replacement));
        }
    }
}
