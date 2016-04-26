using System.Collections.Generic;
using System.Linq;
using Crisp.Interfaces;
using Crisp.Interfaces.Evaluation;
using Crisp.Shared;
using Crisp.Types;

namespace Crisp.Basic
{
    /// <summary>
    /// A function that given a list of named expressions, returns that list as a single evaluable value.
    /// </summary>
    public class LetSpecialForm : SpecialForm
    {
        public override IEnumerable<string> Names => new List<string> {"let"};

        public override ISymbolicExpression Apply(ISymbolicExpression expression, IEvaluator evaluator)
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
                    throw new FunctionApplicationException(
                        $"Bindings specified in a {Name} expression must be in the form of pairs.");
                }

                // The target of each binding must be a symbol.
                if (binding.AsPair().Head.Type != SymbolicExpressionType.Symbol)
                {
                    throw new FunctionApplicationException(
                        $"Bindings specified in a {Name} expression must bind symbols to expressions.");
                }
            }

            // Create new evaluator containing new bindings.
            var newEvaluator = evaluator.Derive(bindings.ToDictionary(b => b.AsPair().Head.AsSymbol().Value, 
                b => b.AsPair().Tail));
            
            return newEvaluator.Evaluate(evaluable);
        }
    }
}
