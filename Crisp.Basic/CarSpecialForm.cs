using Crisp.Core;
using Crisp.Core.Evaluation;
using Crisp.Core.Types;

namespace Crisp.Basic
{
    /// <summary>
    /// Returns the head (first) of a cons cell (pair).
    /// </summary>
    public class CarSpecialForm : SpecialForm
    {
        public override string Name => "car";

        public override SymbolicExpression Apply(SymbolicExpression expression, IEvaluator evaluator)
        {
            expression.ThrowIfNotList(Name); // Takes a list of arguments.

            var arguments = expression.AsPair().Expand();
            arguments.ThrowIfWrongLength(Name, 1); // Must have one argument.

            // Result of evaluation of argument must be a pair.
            var evaluated = evaluator.Evaluate(arguments[0]);
            if (evaluated.Type != SymbolicExpressionType.Pair)
            {
                throw new RuntimeException($"The argument to the function {Name} must be a pair.");
            }

            return evaluated.AsPair().Head;
        }
    }
}
