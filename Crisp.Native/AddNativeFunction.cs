using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Crisp.Core;

namespace Crisp.Native
{
    public class AddNativeFunction : IFunction
    {
        public IFunctionHost Host { get; set; }

        public string Name
        {
            get
            {
                return "add";
            }
        }

        public SymbolicExpression Apply(SymbolicExpression input, Context context)
        {
            var node = input.AsNode(); // Argument list is always a node.

            var head = Host.Evaluate(node.Head, context).AsNumeric(); 
            var tail = Host.Evaluate(node.GoTail().Head, context).AsNumeric();

            return new NumericAtom(head.Value + tail.Value);
        }
    }
}
