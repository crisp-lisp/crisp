using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Crisp.Core;

namespace Crisp.Native
{
    public class AddNativeFunction : INativeFunction
    {
        public INativeFunctionHost Host { get; set; }

        public string Name
        {
            get
            {
                return "add";
            }
        }

        public SymbolicExpression Apply(SymbolicExpression input)
        {
            var node = input.AsNode(); // Argument list as node.

            var leftExpression = Host.Evaluate(node.Head).AsNumeric(); 
            var rightExpression = Host.Evaluate(node.GoTail().Head).AsNumeric();

            return new NumericAtom(leftExpression.Value + rightExpression.Value);
        }
    }
}
