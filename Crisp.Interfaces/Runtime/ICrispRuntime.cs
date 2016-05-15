using Crisp.Interfaces;
using Crisp.Interfaces.Parsing;
using Crisp.Interfaces.Types;

namespace Crisp.Interfaces.Runtime
{
    /// <summary>
    /// Represents a runtime capable of running Crisp programs.
    /// </summary>
    public interface ICrispRuntime
    {
        /// <summary>
        /// Runs a Crisp program against the argument source provided.
        /// </summary>
        /// <param name="argumentSource">The argument source for the program.</param>
        /// <returns></returns>
        ISymbolicExpression Run(IExpressionTreeSource argumentSource);
    }
}