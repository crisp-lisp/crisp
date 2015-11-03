using Crisp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crisp.Native
{
    public class QuoteNativeFunction : IFunction
    {
        public IFunctionHost Host { get; set; }

        public string Name => "quote";

        public SymbolicExpression Apply(SymbolicExpression expression, Context context)
        {
            expression.ThrowIfNotList(Name); // Takes a list of arguments.

            var arguments = expression.AsPair().Expand();
            arguments.ThrowIfWrongLength(Name, 1); // Must have one argument.

            return new ConstantAtom(arguments[0].AsSymbol()); // Argument list is always a node.
        }
    }
}
