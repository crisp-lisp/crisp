using System.Collections.Generic;
using System.Linq;

using Crisp.Core;
using Crisp.Core.Evaluation;
using Crisp.Core.Types;

namespace Crisp.Basic
{
    /// <summary>
    /// Returns a callable function (closure) with the specified parameters and body.
    /// </summary>
    public class LambdaSpecialForm : SpecialForm
    {
        public override string Name => "lambda";

        public override SymbolicExpression Apply(SymbolicExpression expression, IEvaluator evaluator)
        {
            expression.ThrowIfNotList(Name); // Takes a list of arguments.

            var arguments = expression.AsPair().Expand();
            arguments.ThrowIfWrongLength(Name, 2); // Must have two arguments.

            // Parameter list is given in first argument.
            var parameters = arguments[0];

            // Parameter list may be empty (nil) so account for that.
            IList<SymbolicExpression> parameterList = new List<SymbolicExpression>();
            if (parameters.Type != SymbolicExpressionType.Nil)
            {
                if (parameters.Type != SymbolicExpressionType.Pair)
                {
                    throw new RuntimeException(
                        $"Parameter list must be provided as argument 1 to the '{Name}' function.");
                }

                // Ensure only symbols in parameter list.
                parameterList = parameters.AsPair().Expand();
                if (parameterList.Any(p => p.Type != SymbolicExpressionType.Symbol))
                {
                    throw new RuntimeException(
                        $"Parameter list provided to the '{Name}' function must be a list of symbols only.");
                }
            }

            var symbolicParameterList = parameterList.Select(p => (SymbolAtom) p).ToList();
            return new Closure(symbolicParameterList, arguments[1], evaluator);
        }
    }
}
