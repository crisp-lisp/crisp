using System.Collections.Generic;

using Crisp.Core.Tokenizing;
using Crisp.Core.Types;

namespace Crisp.Core.Parsing
{
    /// <summary>
    /// Represents a parser.
    /// </summary>
    public interface IParser
    {
        /// <summary>
        /// Turns a token list into an expression tree.
        /// </summary>
        /// <param name="tokens">The token list to parse.</param>
        /// <returns></returns>
        SymbolicExpression CreateExpressionTree(IList<Token> tokens);
    }
}
