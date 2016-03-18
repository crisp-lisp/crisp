using System.Collections.Generic;
using System.Linq;

using Crisp.Core;
using Crisp.Core.Evaluation;
using Crisp.Core.Types;

namespace Crisp.String
{
    /// <summary>
    /// Represents the basic string replace function.
    /// </summary>
    public class ReplaceSpecialForm : SpecialForm
    {
        public override string Name => "replace";

        public override SymbolicExpression Apply(SymbolicExpression expression, IEvaluator evaluator)
        {
            expression.ThrowIfNotList(Name); // Takes a list of arguments.

            var arguments = expression.AsPair().Expand();
            arguments.ThrowIfWrongLength(Name, 3); // Must have three arguments.

            // Attempt to evaluate every argument to a string.
            var evaluated = arguments.Select(evaluator.Evaluate).ToArray();
            if (evaluated.Any(e => e.Type != SymbolicExpressionType.String))
            {
                throw new RuntimeException(
                    $"The arguments to the function '{Name}' must all evaluate to the string type.");
            }

            var subject = evaluated[0].AsString().Value;
            var search = evaluated[1].AsString().Value;
            var replacement = evaluated[2].AsString().Value;
            
            return new StringAtom(subject.Replace(search, replacement));
        }
    }
}
