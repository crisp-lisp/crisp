using System.Collections.Generic;

using Crisp.Enums;

namespace Crisp.Interfaces.Tokenization
{
    /// <summary>
    /// A token filter, used to remove <see cref="IToken"/> instances with a particular <see cref="TokenType"/>.
    /// </summary>
    public interface ITokenFilter
    {
        /// <summary>
        /// Gets the token types to be filtered.
        /// </summary>
        IEnumerable<TokenType> Filtered { get; }

        /// <summary>
        /// Filters a token list.
        /// </summary>
        /// <param name="tokens">The list of tokens to filter.</param>
        /// <returns></returns>
        IList<IToken> Filter(IList<IToken> tokens);
    }
}
