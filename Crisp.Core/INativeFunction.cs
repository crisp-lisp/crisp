using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crisp.Core
{
    public interface INativeFunction
    {
        string Name { get; }

        INativeFunctionHost Host { get; set; }

        SymbolicExpression Apply(SymbolicExpression input);
    }
}
