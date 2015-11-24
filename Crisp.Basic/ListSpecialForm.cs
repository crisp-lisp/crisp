using Crisp.Core;
using Crisp.Core.Evaluation;
using Crisp.Core.Types;

namespace Crisp.Basic
{
    /// <summary>
    /// Represents the basic list function.
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
