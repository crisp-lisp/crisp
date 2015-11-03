using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Crisp.Core;

namespace Crisp.Native
{
    /// <summary>
    /// Represents the basic function to retrieve the tail of a pair.
    /// </summary>
    public class CdrNativeFunction : IFunction
    {
        public IFunctionHost Host { get; set; }

        public string Name => "cdr";

        public SymbolicExpression Apply(SymbolicExpression expression, Context context)
        {
            expression.ThrowIfNotList(Name); // Takes a list of arguments.

            var arguments = expression.AsPair().Expand();
            arguments.ThrowIfWrongLength(Name, 1); // Must have one argument.

            // Result of evaluation of argument must be a pair.
            var evaluated = Host.Evaluate(arguments[0], context);
            if (evaluated.Type != SymbolicExpressionType.Pair)
            {
                throw new RuntimeException($"The argument to the function {Name} must be a pair.");
            }

            return evaluated.AsPair().Tail;
        }
    }
}
