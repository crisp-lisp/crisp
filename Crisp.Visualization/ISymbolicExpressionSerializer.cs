using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Crisp.Core;

namespace Crisp.Visualization
{
    public interface ISymbolicExpressionSerializer
    {
        string Serialize(SymbolicExpression expression);
    }
}
