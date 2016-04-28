using System.Collections.Generic;

using Crisp.Interfaces.Evaluation;
using Crisp.Interfaces.Types;
using Crisp.Types;

namespace Crisp.Basic
{
    /// <summary>
    /// A special form that given an expression returns true if its value is atomic; false if not.
    /// </summary>
    public class AtomSpecialForm : SpecialForm
    {
        public override IEnumerable<string> Names => new List<string> {"atom", "atomp"};

        public override ISymbolicExpression Apply(ISymbolicExpression expression, IEvaluator evaluator)
        {
            expression.ThrowIfNotList(Name); // Takes a list of arguments.

            var arguments = expression.AsPair().Expand();
            arguments.ThrowIfWrongLength(Name, 1); // Must have one argument.

            var evaluated = evaluator.Evaluate(arguments[0]); // Evaluate argument.

            return evaluated.IsAtomic ? new BooleanAtom(true) : new BooleanAtom(false);
        }
    }
}
