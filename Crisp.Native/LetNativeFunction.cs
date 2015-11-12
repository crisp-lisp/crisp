using System.Linq;

using Crisp.Core;
using Crisp.Core.Evaluation;

namespace Crisp.Native
{
    /// <summary>
    /// Represents the basic function to bind symbols to expressions.
    /// </summary>
    public class LetNativeFunction : IFunction
    {
        public IEvaluator Host { get; set; }

        public string Name => "let";

        public SymbolicExpression Apply(SymbolicExpression expression, Context context)
        {
            expression.ThrowIfNotList(Name); // Takes a list of arguments.

            var arguments = expression.AsPair().Expand();
            arguments.ThrowIfShorterThanLength(Name, 1); // Must have at least one argument (the body).

            var evaluable = arguments[0];

            var bindings = arguments.Where(a => a != evaluable);
            var newContext = context;
            foreach (var binding in bindings)
            {
                // Bindings must be formatted as pairs.
                if (binding.Type != SymbolicExpressionType.Pair)
                {
                    throw new RuntimeException(
                        $"Bindings specified in a {Name} expression must be in the form of pairs.");
                }

                // The target of each binding must be a symbol.
                if (binding.AsPair().Head.Type != SymbolicExpressionType.Symbol)
                {
                    throw new RuntimeException(
                        $"Bindings specified in a {Name} expression must bind symbols to expressions.");
                }

                newContext = newContext.Bind(binding.AsPair().Head.AsSymbol(),
                    Host.Evaluate(binding.AsPair().Tail, context));
            }

            return Host.Evaluate(evaluable, newContext);
        }
    }
}
