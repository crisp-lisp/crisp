using Crisp.Core;
using Crisp.Core.Evaluation;

namespace Crisp.Basic
{
    /// <summary>
    /// Represents the basic atomic test function.
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

            return evaluated.IsAtomic ? evaluator.Evaluate(SymbolAtom.True) 
                : evaluator.Evaluate(SymbolAtom.False);
        }
    }
}
