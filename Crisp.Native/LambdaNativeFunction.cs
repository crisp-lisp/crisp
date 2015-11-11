using System.Linq;
using Crisp.Core;

namespace Crisp.Native
{
    /// <summary>
    /// Represents the lambda creation function.
    /// </summary>
    public class LambdaNativeFunction : IFunction
    {
        public IFunctionHost Host { get; set; }

        public string Name => "lambda";

        public SymbolicExpression Apply(SymbolicExpression expression, Context context)
        {
            expression.ThrowIfNotList(Name); // Takes a list of arguments.

            var arguments = expression.AsPair().Expand();
            arguments.ThrowIfWrongLength(Name, 2); // Must have two arguments.

            // Ensure parameter list was given.
            var parameters = arguments[0];
            if (parameters.Type != SymbolicExpressionType.Pair)
            {
                throw new RuntimeException($"Parameter list must be provided as argument 1 to the '{Name}' function.");
            }

            // Ensure only symbols in parameter list.
            var parameterList = parameters.AsPair().Expand();
            if (parameterList.Any(p => p.Type != SymbolicExpressionType.Symbol))
            {
                throw new RuntimeException(
                    $"Parameter list provided to the '{Name}' function must be a list of symbols only.");
            }

            var symbolicParameterList = parameterList.Select(p => (SymbolAtom) p).ToList();
            return new Closure(Host, symbolicParameterList, arguments[1], context);
        }
    }
}
