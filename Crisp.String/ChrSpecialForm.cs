using System;
using System.Collections.Generic;
using System.Linq;
using Crisp.Enums;
using Crisp.Interfaces;
using Crisp.Interfaces.Evaluation;
using Crisp.Shared;
using Crisp.Types;

namespace Crisp.String
{
    /// <summary>
    /// A function that converts from a numeric ASCII code to its character equivalent.
    /// </summary>
    public class ChrSpecialForm : SpecialForm
    {
        public override IEnumerable<string> Names => new List<string> {"chr"};

        public override ISymbolicExpression Apply(ISymbolicExpression expression, IEvaluator evaluator)
        {
            expression.ThrowIfNotList(Name); // Takes a list of arguments.

            var arguments = expression.AsPair().Expand();
            arguments.ThrowIfWrongLength(Name, 1); // Must have one argument.

            // Attempt to evaluate every argument to a number.
            var evaluated = arguments.Select(evaluator.Evaluate).ToArray();
            if (evaluated.Any(e => e.Type != SymbolicExpressionType.Numeric))
            {
                throw new FunctionApplicationException(
                    $"The arguments to the function '{Name}' must all evaluate to the numeric type.");
            }

            return new StringAtom(Convert.ToChar(Convert.ToByte(evaluated[0].AsNumeric().Value)).ToString());
        }
    }
}
