using Crisp.Core;
using Crisp.Core.Evaluation;
using Crisp.Core.Types;

namespace Crisp.Basic
{
    /// <summary>
    /// Represents the basic equality test function.
    /// </summary>
    public class EqSpecialForm : SpecialForm
    {
        public override string Name => "eq";

        public override SymbolicExpression Apply(SymbolicExpression expression, IEvaluator evaluator)
        {
            expression.ThrowIfNotList(Name); // Takes a list of arguments.

            var arguments = expression.AsPair().Expand();
            arguments.ThrowIfWrongLength(Name, 2); // Must have two arguments.

            // Evaluate both parameters.
            var x = evaluator.Evaluate(arguments[0]);
            var y = evaluator.Evaluate(arguments[1]);

            return x.Equals(y)
                ? new BooleanAtom(true)
                : new BooleanAtom(false);
        }
    }
}
