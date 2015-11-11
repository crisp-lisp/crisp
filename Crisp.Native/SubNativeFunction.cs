using System.Linq;

using Crisp.Core;

namespace Crisp.Native
{
    /// <summary>
    /// Represents the basic subtraction function.
    /// </summary>
    public class SubNativeFunction : IFunction
    {
        public IEvaluator Host { get; set; }

        public string Name => "sub";

        public SymbolicExpression Apply(SymbolicExpression expression, Context context)
        {
            expression.ThrowIfNotList(Name); // Takes a list of arguments.

            var arguments = expression.AsPair().Expand();
            arguments.ThrowIfWrongLength(Name, 2); // Must have two arguments.

            // Attempt to evaluate every argument to a number.
            var evaluated = arguments.Select(a => Host.Evaluate(a, context)).ToArray();
            if (evaluated.Any(e => e.Type != SymbolicExpressionType.Numeric))
            {
                throw new RuntimeException(
                    $"The arguments to the function '{Name}' must all evaluate to the numeric type.");
            }

            return new NumericAtom(evaluated[0].AsNumeric().Value - evaluated[1].AsNumeric().Value);
        }
    }
}
