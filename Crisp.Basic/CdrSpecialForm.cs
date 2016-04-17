using Crisp.Shared;
using Crisp.Types;

namespace Crisp.Basic
{
    /// <summary>
    /// Returns the tail (rest) of a cons cell (pair).
    /// </summary>
    public class CdrSpecialForm : SpecialForm
    {
        public override string Name => "cdr";

        public override ISymbolicExpression Apply(ISymbolicExpression expression, IEvaluator evaluator)
        {
            expression.ThrowIfNotList(Name); // Takes a list of arguments.

            var arguments = expression.AsPair().Expand();
            arguments.ThrowIfWrongLength(Name, 1); // Must have one argument.

            // Result of evaluation of argument must be a pair.
            var evaluated = evaluator.Evaluate(arguments[0]);
            if (evaluated.Type != SymbolicExpressionType.Pair)
            {
                throw new FunctionApplicationException($"The argument to the function {Name} must be a pair.");
            }

            return evaluated.AsPair().Tail;
        }
    }
}
