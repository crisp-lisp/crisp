using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crisp.Core.Types;

namespace Crisp.Core.Runtime
{
    public interface ICrispRuntime
    {
        SymbolicExpression EvaluateSource(string source, SymbolicExpression arguments);

        SymbolicExpression EvaluateSource(string source, string arguments);

        SymbolicExpression EvaluateSourceFile(string filepath, SymbolicExpression arguments);

        SymbolicExpression EvaluateSourceFile(string filepath, string arguments);
    }
}
