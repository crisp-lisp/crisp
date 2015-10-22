using Crisp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crisp.Native
{
    public class AtomNativeFunction : IFunction
    {
        public IFunctionHost Host { get; set; }

        public string Name
        {
            get
            {
                return "atom";
            }
        }

        public SymbolicExpression Apply(SymbolicExpression input, Context context)
        {
            var node = input.AsPair(); // Argument list is always a node.

            var expression = Host.Evaluate(node.Head, context);

            return expression.IsAtomic ? SymbolAtom.True : SymbolAtom.False;
        }
    }
}
