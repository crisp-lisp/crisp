using System.Linq;

using Crisp.Core;
using Crisp.Core.Evaluation;

namespace Crisp.Basic
{
    /// <summary>
    /// Represents the basic function to bind symbols to expressions.
    /// </summary>
    public class LetrecSpecialForm : SpecialForm
    {
        public override string Name => "letrec";

        public override SymbolicExpression Apply(SymbolicExpression expression, IEvaluator evaluator)
        {
            expression.ThrowIfNotList(Name); // Takes a list of arguments.

            var arguments = expression.AsPair().Expand();
            arguments.ThrowIfShorterThanLength(Name, 1); // Must have at least one argument (the body).

            var evaluable = arguments[0]; // This is the actual evaluable bit.

            var bindings = arguments.Where(a => a != evaluable).ToArray();
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
            }

            // Mutate existing evaluator.
            var bindingDictionary = bindings.ToDictionary(b => b.AsPair().Head.AsSymbol(), b => b.AsPair().Tail);
            evaluator.MutableBind(bindingDictionary);
            
            return evaluator.Evaluate(evaluable);
        }
    }
}
