using System.Collections.Generic;
using System.Linq;
using Crisp.Enums;
using Crisp.Interfaces;
using Crisp.Interfaces.Evaluation;
using Crisp.Shared;
using Crisp.Types;

namespace Crisp.Basic
{
    /// <summary>
    /// A function that given an expression, returns that expression as an evaluable value.
    /// </summary>
    public class LambdaSpecialForm : SpecialForm
    {
        public override IEnumerable<string> Names => new List<string> {"lambda"};

        public override ISymbolicExpression Apply(ISymbolicExpression expression, IEvaluator evaluator)
        {
            expression.ThrowIfNotList(Name); // Takes a list of arguments.

            var arguments = expression.AsPair().Expand();
            arguments.ThrowIfWrongLength(Name, 2); // Must have two arguments.

            // Parameter list is given in first argument.
            var parameters = arguments[0];

            // Parameter list may be empty (nil) so account for that.
            IList<ISymbolicExpression> parameterList = new List<ISymbolicExpression>();
            if (parameters.Type != SymbolicExpressionType.Nil)
            {
                if (parameters.Type != SymbolicExpressionType.Pair)
                {
                    throw new FunctionApplicationException(
                        $"Parameter list must be provided as argument 1 to the '{Name}' function.");
                }

                // Ensure only symbols in parameter list.
                parameterList = parameters.AsPair().Expand();
                if (parameterList.Any(p => p.Type != SymbolicExpressionType.Symbol))
                {
                    throw new FunctionApplicationException(
                        $"Parameter list provided to the '{Name}' function must be a list of symbols only.");
                }
            }

            var symbolicParameterList = parameterList.Select(p => (SymbolAtom) p).ToList();
            return new Closure(symbolicParameterList, arguments[1], evaluator);
        }
    }
}
