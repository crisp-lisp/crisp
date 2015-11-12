using Crisp.Core;
using Crisp.Core.Evaluation;

namespace Crisp.Native
{
    /// <summary>
    /// Represents the basic function to create a pair from two expressions.
    /// </summary>
    public class ConsSpecialForm : SpecialForm
    {
        public override string Name => "cons";

        public override SymbolicExpression Apply(SymbolicExpression expression, IEvaluator evaluator)
        {
            expression.ThrowIfNotList(Name); // Takes a list of arguments.

            var arguments = expression.AsPair().Expand();
            arguments.ThrowIfWrongLength(Name, 2); // Must have two arguments.

            var head = evaluator.Evaluate(arguments[0]);
            var tail = evaluator.Evaluate(arguments[1]);

            return new Pair(head, tail);
        }
    }
}
