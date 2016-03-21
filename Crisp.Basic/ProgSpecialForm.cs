using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crisp.Core;
using Crisp.Core.Evaluation;
using Crisp.Core.Types;

namespace Crisp.Basic
{
    /// <summary>
    /// Represents the basic prog special function.
    /// </summary>
    public class ProgSpecialForm : SpecialForm
    {
        public override string Name => "prog";

        public override SymbolicExpression Apply(SymbolicExpression expression, IEvaluator evaluator)
        {
            expression.ThrowIfNotList(Name); // Takes a list of arguments.

            var evaluable = expression.AsPair().Expand();
            var evaluated = evaluable.Select(evaluator.Evaluate); // Evaluate all arguments once.

            return evaluated.Last(); // Return last item.
        }
    }
}
