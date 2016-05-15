using System.Collections.Generic;

using Crisp.Interfaces.Evaluation;
using Crisp.Interfaces.Types;
using Crisp.Types;

namespace Crisp.Basic
{
    /// <summary>
    /// A special form that given two expressions returns a value pair consisting of their values.
    /// </summary>
    public class ConsSpecialForm : SpecialForm
    {
        public override IEnumerable<string> Names => new List<string> {"cons"};

        public override ISymbolicExpression Apply(ISymbolicExpression expression, IEvaluator evaluator)
        {
            expression.ThrowIfNotList(Name); // Takes a list of arguments.

            var arguments = expression.AsPair().Expand();
            arguments.ThrowIfWrongLength(Name, 2); // Must have two arguments.

            // Evaluate arguments.
            var head = evaluator.Evaluate(arguments[0]);
            var tail = evaluator.Evaluate(arguments[1]);

            // Return pair with results as head and tail.
            return new Pair(head, tail);
        }
    }
}
