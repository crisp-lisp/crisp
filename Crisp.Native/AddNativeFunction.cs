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
        public string Name
        {
            get
            {
                return "add";
            }
        }

        public SymbolicExpression Apply(SymbolicExpression input)
        {
            return new SymbolicExpression("JHI!");
        }
    }
}
