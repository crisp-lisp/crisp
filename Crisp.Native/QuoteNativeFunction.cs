using Crisp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crisp.Native
{
    public class QuoteNativeFunction : IFunction
    {
        public IFunctionHost Host { get; set; }

        public string Name
        {
            get
            {
                return "quote";
            }
        }

        public SymbolicExpression Apply(SymbolicExpression input, Context context)
        {
            return input.AsNode().Head; // Argument list is always a node.
        }
    }
}
