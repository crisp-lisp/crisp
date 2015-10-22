using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Crisp.Core;

namespace Crisp.Native
{
    public class CdrNativeFunction : IFunction
    {
        public IFunctionHost Host { get; set; }

        public string Name
        {
            get
            {
                return "cdr";
            }
        }

        public SymbolicExpression Apply(SymbolicExpression input, Context context)
        {
            var node = input.AsPair(); // Argument list is always a node.

            var expression = Host.Evaluate(node.Head, context);

            return Host.Evaluate(expression.AsPair().Tail, context);
        }
    }
}
