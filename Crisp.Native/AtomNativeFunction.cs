using Crisp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crisp.Native
{
    public class AtomNativeFunction : INativeFunction
    {
        public INativeFunctionHost Host { get; set; }

        public string Name
        {
            get
            {
                return "atom";
            }
        }

        public SymbolicExpression Apply(SymbolicExpression input)
        {
            var node = input.AsNode();

            var expression = Host.Evaluate(node.Head);

            return expression.IsAtomic ? SymbolAtom.True : SymbolAtom.False;
        }
    }
}
