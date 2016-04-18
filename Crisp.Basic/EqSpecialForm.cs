using System.Collections.Generic;

using Crisp.Shared;
using Crisp.Types;

namespace Crisp.Basic
{
    /// <summary>
    /// A function that given two expressions returns true if their values are equal; false if not.
    /// </summary>
    public class EqSpecialForm : SpecialForm
    {
        public override IEnumerable<string> Names => new List<string> {"eq"};

        public override ISymbolicExpression Apply(ISymbolicExpression expression, IEvaluator evaluator)
        {
            expression.ThrowIfNotList(Name); // Takes a list of arguments.

            var arguments = expression.AsPair().Expand();
            arguments.ThrowIfWrongLength(Name, 2); // Must have two arguments.

            // Evaluate both parameters.
            var x = evaluator.Evaluate(arguments[0]);
            var y = evaluator.Evaluate(arguments[1]);

            return x.Equals(y) && x.IsAtomic && y.IsAtomic // Non-atoms can never be compared.
                ? new BooleanAtom(true)
                : new BooleanAtom(false);
        }
    }
}
