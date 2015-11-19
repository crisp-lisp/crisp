using Crisp.Core;
using Crisp.Core.Evaluation;

namespace Crisp.Basic
{
    /// <summary>
    /// Represents the basic atomic test function.
    /// </summary>
    public class IfSpecialForm : SpecialForm
    {
        public override string Name => "if";

        public override SymbolicExpression Apply(SymbolicExpression expression, IEvaluator evaluator)
        {
            expression.ThrowIfNotList(Name); // Takes a list of arguments.

            var arguments = expression.AsPair().Expand();
            arguments.ThrowIfWrongLength(Name, 3); // Must have three arguments.

            // Grab true and false bindings from context.
            var t = evaluator.Evaluate(SymbolAtom.True);
            var f = evaluator.Evaluate(SymbolAtom.False);

            // Evaluate predicate.
            var predicate = evaluator.Evaluate(arguments[0]);
            if (predicate != t && predicate != f)
            {
                throw new RuntimeException($"The first argument to the function '{Name}' must evaluate to a boolean special atom.");
            }
            
            return predicate == t ? evaluator.Evaluate(arguments[1]) 
                : evaluator.Evaluate(arguments[2]);
        }
    }
}
