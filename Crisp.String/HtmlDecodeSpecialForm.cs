using System.Linq;
using System.Web;

using Crisp.Core;
using Crisp.Core.Evaluation;
using Crisp.Core.Types;

namespace Crisp.String
{
    /*
     * Note: This function should be implemented in Crisp as it's not necessary to implement it at this level.
     */

    /// <summary>
    /// Represents the basic HTML decode function.
    /// </summary>
    public class HtmlDecodeSpecialForm : SpecialForm // TODO: Implement in Crisp.
    {
        public override string Name => "html-decode";

        public override SymbolicExpression Apply(SymbolicExpression expression, IEvaluator evaluator)
        {
            expression.ThrowIfNotList(Name); // Takes a list of arguments.

            var arguments = expression.AsPair().Expand();
            arguments.ThrowIfWrongLength(Name, 1); // Must have one argument.

            // Attempt to evaluate every argument to a string.
            var evaluated = arguments.Select(evaluator.Evaluate).ToArray();
            if (evaluated.Any(e => e.Type != SymbolicExpressionType.String))
            {
                throw new RuntimeException(
                    $"The arguments to the function '{Name}' must all evaluate to the string type.");
            }

            return new StringAtom(HttpUtility.HtmlDecode(evaluated[0].AsString().Value));
        }
    }
}
