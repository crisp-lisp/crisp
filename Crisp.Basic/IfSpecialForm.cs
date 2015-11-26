using Crisp.Core;
using Crisp.Core.Evaluation;
using Crisp.Core.Types;

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
            
            var t = new BooleanAtom(true);
            var f = new BooleanAtom(false);

            // Evaluate predicate.
            var predicate = evaluator.Evaluate(arguments[0]);
            if (predicate.Equals(t) && predicate.Equals(f))
            {
                throw new RuntimeException($"The first argument to the function '{Name}' must evaluate to a boolean special atom.");
            }
            
            return predicate.Equals(t) ? evaluator.Evaluate(arguments[1]) 
                : evaluator.Evaluate(arguments[2]);
        }
    }
}
