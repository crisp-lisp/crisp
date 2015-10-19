using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Crisp.Core;

namespace Crisp.Native
{
    public class CarNativeFunction : INativeFunction
    {
        public INativeFunctionHost Host { get; set; }

        public string Name
        {
            get
            {
                return "car";
            }
        }

        public SymbolicExpression Apply(SymbolicExpression input)
        {
            var node = input.AsNode();
            
            return Host.Evaluate(node.GoHead().GoHead());
        }
    }
}
