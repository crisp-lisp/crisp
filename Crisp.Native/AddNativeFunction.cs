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
            var leftExpression = Convert.ToInt32(input.LeftExpression.IsAtomic ? input.LeftExpression.Value
                : Host.Evaluate(input.LeftExpression).Value);

            var rightExpression = Convert.ToInt32(input.RightExpression.LeftExpression.IsAtomic ? input.RightExpression.LeftExpression.Value
                : Host.Evaluate(input.RightExpression.LeftExpression).Value);

            return new SymbolicExpression(leftExpression + rightExpression);
        }
    }
}
