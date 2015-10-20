using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Crisp.Core;

namespace Crisp.Native
{
    public class CdrNativeFunction : INativeFunction
    {
        public INativeFunctionHost Host { get; set; }

        public string Name
        {
            get
            {
                return "cdr";
            }
        }

        public SymbolicExpression Apply(SymbolicExpression input)
        {
            var node = input.AsNode(); // Argument list is always a node.

            var expression = Host.Evaluate(node.Head);

            return Host.Evaluate(expression.AsNode().Tail);
        }
    }
}
