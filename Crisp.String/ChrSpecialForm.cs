using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crisp.Core;
using Crisp.Core.Evaluation;
using Crisp.Core.Types;

namespace Crisp.String
{
    /// <summary>
    /// An implementation of a function to convert from a numeric ASCII code to its character equivalent.
    /// </summary>
    public class ChrSpecialForm : SpecialForm
    {
        public override string Name => "chr";

        public override SymbolicExpression Apply(SymbolicExpression expression, IEvaluator evaluator)
        {
            expression.ThrowIfNotList(Name); // Takes a list of arguments.

            var arguments = expression.AsPair().Expand();
            arguments.ThrowIfWrongLength(Name, 1); // Must have one argument.

            // Attempt to evaluate every argument to a number.
            var evaluated = arguments.Select(evaluator.Evaluate).ToArray();
            if (evaluated.Any(e => e.Type != SymbolicExpressionType.Numeric))
            {
                throw new RuntimeException(
                    $"The arguments to the function '{Name}' must all evaluate to the numeric type.");
            }

            return new StringAtom(Convert.ToChar(Convert.ToByte(evaluated[0].AsNumeric().Value)).ToString());
        }
    }
}
