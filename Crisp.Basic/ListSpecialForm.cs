using Crisp.Core;
using Crisp.Core.Evaluation;
using Crisp.Core.Types;

namespace Crisp.Basic
{
    /// <summary>
    /// A function that returns its arguments as a proper list.
    /// </summary>
    public class ListSpecialForm : SpecialForm
    {
        public override string Name => "list";

        public override SymbolicExpression Apply(SymbolicExpression expression, IEvaluator evaluator)
        {
            expression.ThrowIfNotList(Name); // Takes a list of arguments.
            return evaluator.Evaluate(expression.AsPair());
        }
    }
}
