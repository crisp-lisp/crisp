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
            return new SymbolicExpression(Convert.ToInt32(input.LeftExpression.Value) + Convert.ToInt32(input.RightExpression.LeftExpression.Value));
        }
    }
}
