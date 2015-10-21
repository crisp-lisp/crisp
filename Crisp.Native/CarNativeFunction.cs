using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Crisp.Core;

namespace Crisp.Native
{
    public class CarNativeFunction : IFunction
    {
        public IFunctionHost Host { get; set; }

        public string Name
        {
            get
            {
                return "car";
            }
        }

        public SymbolicExpression Apply(SymbolicExpression input, Context context)
        {
            var node = input.AsNode(); // Argument list is always a node.

            var expression = Host.Evaluate(node.Head, context);

            return expression.AsNode().Head;
        }
    }
}
