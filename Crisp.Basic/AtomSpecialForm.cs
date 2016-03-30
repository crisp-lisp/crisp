using Crisp.Core;
using Crisp.Core.Evaluation;
using Crisp.Core.Types;

namespace Crisp.Basic
{
    /// <summary>
    /// Returns a true boolean atom if the given expression evaluates to an atom. Otherwise returns a false boolean
    /// atom.
    /// </summary>
    public class AtomSpecialForm : SpecialForm
    {
        public override string Name => "atom";

        public override SymbolicExpression Apply(SymbolicExpression expression, IEvaluator evaluator)
        {
            expression.ThrowIfNotList(Name); // Takes a list of arguments.

            var arguments = expression.AsPair().Expand();
            arguments.ThrowIfWrongLength(Name, 1); // Must have one argument.

            var evaluated = evaluator.Evaluate(arguments[0]);

            return evaluated.IsAtomic ? new BooleanAtom(true)
                : new BooleanAtom(false);
        }
    }
}
