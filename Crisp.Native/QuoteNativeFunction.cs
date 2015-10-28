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

        public SymbolicExpression Apply(SymbolicExpression input, Context context)
        {
            return new ConstantAtom(input.AsPair().Head.AsSymbol()); // Argument list is always a node.
        }
    }
}
