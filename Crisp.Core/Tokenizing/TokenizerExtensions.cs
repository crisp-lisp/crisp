using System.Collections.Generic;
using System.Linq;

namespace Crisp.Core.Tokenizing
{
    /// <summary>
    /// Contains useful extension methods for tokenization.
    /// </summary>
    public static class TokenizerExtensions
    {
        /// <summary>
        /// Removes specific types of token from a token list.
        /// </summary>
        /// <param name="tokens">The token list to act on.</param>
        /// <param name="tokenTypes">The token types to remove.</param>
        /// <returns></returns>
        public static IList<Token> RemoveTokens(this IList<Token> tokens, params TokenType[] tokenTypes)
        {
            return tokens.Where(t => !tokenTypes.Contains(t.Type)).ToList();
        }
    }
}
