using System.Collections.Generic;
using System.Linq;
using Crisp.Interfaces;
using Crisp.Interfaces.Evaluation;
using Crisp.Shared;
using Crisp.Types;

namespace Crisp.String
{
    /// <summary>
    /// Returns a new string atom consisting of two string atoms concatenated together.
    /// </summary>
    public class ConcatSpecialForm : SpecialForm
    {
        public override IEnumerable<string> Names => new List<string> {"concat"};

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

            var concat = evaluated[0].AsString().Value + evaluated[1].AsString().Value;
            return new StringAtom(concat);
        }
    }
}
