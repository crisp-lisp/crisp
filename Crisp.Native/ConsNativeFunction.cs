using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Crisp.Core;

namespace Crisp.Native
{
    public class ConsNativeFunction : INativeFunction
    {
        public INativeFunctionHost Host { get; set; }

        public string Name
        {
            get
            {
                return "cons";
            }
        }

        public SymbolicExpression Apply(SymbolicExpression input)
        {
            return new SymbolicExpression(
                Host.Evaluate(input.LeftExpression),
                Host.Evaluate(input.RightExpression.LeftExpression));
        }
    }
}
