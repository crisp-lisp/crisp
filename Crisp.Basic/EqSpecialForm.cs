using Crisp.Shared;
using Crisp.Types;

namespace Crisp.Basic
{
    /// <summary>
    /// Returns a true boolean atom if the two given arguments are considered equal. Otherwise returns a false boolean
    /// atom.
    /// </summary>
    public class EqSpecialForm : SpecialForm
    {
        public override string Name => "eq";

        public override ISymbolicExpression Apply(ISymbolicExpression expression, IEvaluator evaluator)
        {
            expression.ThrowIfNotList(Name); // Takes a list of arguments.

            var arguments = expression.AsPair().Expand();
            arguments.ThrowIfWrongLength(Name, 2); // Must have two arguments.

            // Evaluate both parameters.
            var x = evaluator.Evaluate(arguments[0]);
            var y = evaluator.Evaluate(arguments[1]);

            return x.Equals(y) && x.IsAtomic && y.IsAtomic // Non-atoms can never be compared.
                ? new BooleanAtom(true)
                : new BooleanAtom(false);
        }
    }
}
