using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Crisp.Core;

namespace Crisp.Native
{
    public class ConsNativeFunction : IFunction
    {
        public IFunctionHost Host { get; set; }

        public string Name => "cons";

        public SymbolicExpression Apply(SymbolicExpression input, Context context)
        {
            var node = input.AsPair(); // Argument list is always a node.

            var head = Host.Evaluate(node.Head, context);
            var tail = Host.Evaluate(node.GoTail().Head, context);

            return new Pair(head, tail);
        }
    }
}
