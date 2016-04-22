using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crisp.Shared
{
    public interface IBinding
    {
        /// <summary>
        /// Gets the name that is bound to the expression.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the expression that is bound to the symbol.
        /// </summary>
        ISymbolicExpression Value { get; }
    }

}
