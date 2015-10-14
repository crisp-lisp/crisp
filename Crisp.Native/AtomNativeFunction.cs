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
            var expression = Host.Evaluate(input.LeftExpression);

            return new SymbolicExpression(expression.IsAtomic ? "T" : "F");
        }
    }
}
