using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Crisp.Core;

namespace Crisp.Native
{
    public class LetNativeFunction : IFunction
    {
        public IFunctionHost Host { get; set; }

        public string Name => "let";

        public SymbolicExpression Apply(SymbolicExpression input, Context context)
        {
            var node = input.AsPair(); // Argument list is always a node.

            var exp = node.Head;
            var name = node.GoTail().GoHead().Head;
            var value = node.GoTail().GoHead().Tail;

            var ct = context.Bind(name.AsSymbol(), value);
            var result = Host.Evaluate(exp, ct);

            return result;
        }
    }
}
